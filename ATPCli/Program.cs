using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace ATPCli
{
    /// <summary>
    /// CRUD CLI for the ATP plugin DB (AED_ATPLUGIN001). Mirrors the SQL the WinForms
    /// (ServiceItem_Form, ServiceContract_Form) emit so an AI can drive end-to-end
    /// create/read/update/delete cycles without UI automation.
    ///
    /// Usage examples:
    ///   atp item create --tag SI-TEST-001 --debtor 3000-A0084 --stock iR-ADV
    ///   atp item read   --tag SI-TEST-001
    ///   atp item audit  --tag SI-TEST-001
    ///   atp item update --tag SI-TEST-001 --desc "Updated copier"
    ///   atp item delete --tag SI-TEST-001
    ///   atp item list   --top 10
    ///   atp meter add   --tag SI-TEST-001 --type 201-BK --rate 0.0285 --min 50
    ///   atp meter list  --tag SI-TEST-001
    ///   atp contract create --code SC-TEST-001 --debtor 3000-A0084 --type WARR
    ///   atp schema verify
    ///   atp cleanup --prefix SI-TEST- --confirm
    ///   atp sql "SELECT TOP 5 ServiceItemCode, Modified FROM zSCP_ServiceItem ORDER BY Modified DESC"
    /// </summary>
    internal static class Program
    {
        static int Main(string[] args)
        {
            try
            {
                if (args.Length == 0 || IsHelp(args[0])) { PrintHelp(); return args.Length == 0 ? 1 : 0; }

                string cmd = args[0].ToLowerInvariant();

                // Single-word commands consume flags starting at args[1]; noun/verb commands at args[2].
                if (cmd == "cleanup") return CleanupCommand(ParseFlags(args, 1));
                if (cmd == "sql")     return SqlCommand(args.Length > 1 ? args[1] : "");
                if (cmd == "help")    { PrintHelp(); return 0; }

                string sub = args.Length > 1 ? args[1].ToLowerInvariant() : "";
                Dictionary<string, string> flags = ParseFlags(args, 2);
                bool jsonOut = flags.ContainsKey("json");

                switch (cmd)
                {
                    case "item":        return ItemCommand(sub, flags, jsonOut);
                    case "contract":    return ContractCommand(sub, flags, jsonOut);
                    case "meter":       return MeterCommand(sub, flags, jsonOut);
                    case "meter-trans": return MeterTransCommand(sub, flags, jsonOut);
                    case "schema":      return SchemaCommand(sub, flags, jsonOut);
                    default:
                        Stderr("Unknown command: " + cmd);
                        PrintHelp();
                        return 2;
                }
            }
            catch (SqlException sx) { Stderr("SQL error " + sx.Number + ": " + sx.Message); return 3; }
            catch (FormatException fx) { Stderr("Bad argument: " + fx.Message); return 2; }
            catch (ArgumentException ax) { Stderr("Bad argument: " + ax.Message); return 2; }
            catch (Exception ex) { Stderr("Error: " + ex.Message); return 1; }
        }

        // ---------------- ITEM ----------------

        private static int ItemCommand(string sub, Dictionary<string, string> f, bool jsonOut)
        {
            switch (sub)
            {
                case "list":   return ItemList(f, jsonOut);
                case "count":  return ItemCount();
                case "create": return ItemCreate(f, jsonOut);
                case "read":   return ItemRead(f, jsonOut);
                case "update": return ItemUpdate(f, jsonOut);
                case "delete": return ItemDelete(f);
                case "audit":  return ItemAudit(f, jsonOut);
                default: Stderr("Unknown item subcommand: " + sub); return 2;
            }
        }

        private static int ItemList(Dictionary<string, string> f, bool jsonOut)
        {
            int top = ParseInt(f, "top", 25);
            string like = Get(f, "like", null);
            string where = like == null ? "" : " WHERE ServiceItemCode LIKE @like";
            string sql = "SELECT TOP " + top + " ServiceItemKey, ServiceItemCode, ISNULL(StockCode,'') AS StockCode, " +
                         "ISNULL(DebtorCode,'') AS DebtorCode, ISNULL([Description],'') AS [Description], " +
                         "Inactive, Created, Modified FROM [dbo].[zSCP_ServiceItem]" + where +
                         " ORDER BY ServiceItemKey DESC";
            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlCommand c = new SqlCommand(sql, cn))
            {
                if (like != null) c.Parameters.AddWithValue("@like", like);
                using (SqlDataReader r = c.ExecuteReader())
                {
                    PrintReader(r, jsonOut);
                }
            }}
            return 0;
        }

        private static int ItemCount()
        {
            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlCommand c = new SqlCommand("SELECT COUNT(*) FROM [dbo].[zSCP_ServiceItem]", cn))
            { Console.WriteLine(c.ExecuteScalar()); }}
            return 0;
        }

        private static int ItemCreate(Dictionary<string, string> f, bool jsonOut)
        {
            string tag = RequireFlag(f, "tag");
            string user = Get(f, "user", DefaultUser());
            DateTime now = DateTime.UtcNow;

            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlTransaction tx = cn.BeginTransaction("AtpCliItemCreate"))
            {
                try
                {
                    string sql = "INSERT INTO [dbo].[zSCP_ServiceItem] " +
                        "(ServiceItemCode, StockCode, ServiceItemGradeCode, PurchaseDate, UnitPrice, RefNo, [Description], " +
                        " ContractNo, ServiceStartDate, ServiceExpiryDate, DebtorCode, StaffCode, TermCode, AreaCode, " +
                        " Address1, Attention, Note, " +
                        " PMIntervalType, PMIntervalValue, PMActive, " +
                        " DepartmentCode, JobCode, StockLocationCode, Inactive, " +
                        " Created, Modified, CreatedBy, ModifiedBy) VALUES " +
                        "(@code, @stock, @grade, @pdate, @price, @refno, @desc, " +
                        " @contract, @sstart, @send, @debtor, @agent, @term, @area, " +
                        " @addr1, @attn, @note, " +
                        " @pmtype, @pmval, @pmactive, " +
                        " @dept, @job, @loc, @inactive, " +
                        " @created, @modified, @user, @user); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                    using (SqlCommand cmd = new SqlCommand(sql, cn, tx))
                    {
                        cmd.Parameters.AddWithValue("@code", tag);
                        cmd.Parameters.AddWithValue("@stock", Get(f, "stock", ""));
                        cmd.Parameters.AddWithValue("@grade", Get(f, "grade", ""));
                        cmd.Parameters.AddWithValue("@pdate", ParseDate(f, "purchase-date", DateTime.Today));
                        cmd.Parameters.AddWithValue("@price", ParseDecimal(f, "price", 0m));
                        cmd.Parameters.AddWithValue("@refno", Get(f, "ref", ""));
                        cmd.Parameters.AddWithValue("@desc", Get(f, "desc", ""));
                        cmd.Parameters.AddWithValue("@contract", Get(f, "contract", ""));
                        cmd.Parameters.AddWithValue("@sstart", ParseDate(f, "start", DateTime.Today));
                        cmd.Parameters.AddWithValue("@send", ParseDate(f, "end", DateTime.Today.AddYears(1)));
                        cmd.Parameters.AddWithValue("@debtor", Get(f, "debtor", ""));
                        cmd.Parameters.AddWithValue("@agent", Get(f, "agent", ""));
                        cmd.Parameters.AddWithValue("@term", Get(f, "term", ""));
                        cmd.Parameters.AddWithValue("@area", Get(f, "area", ""));
                        cmd.Parameters.AddWithValue("@addr1", Get(f, "address", ""));
                        cmd.Parameters.AddWithValue("@attn", Get(f, "attention", ""));
                        cmd.Parameters.AddWithValue("@note", Get(f, "note", ""));
                        cmd.Parameters.AddWithValue("@pmtype", Get(f, "pm-type", "NONE"));
                        cmd.Parameters.AddWithValue("@pmval", ParseInt(f, "pm-interval", 0));
                        cmd.Parameters.AddWithValue("@pmactive", Get(f, "pm-active", "N"));
                        cmd.Parameters.AddWithValue("@dept", Get(f, "department", ""));
                        cmd.Parameters.AddWithValue("@job", Get(f, "job", ""));
                        cmd.Parameters.AddWithValue("@loc", Get(f, "location", ""));
                        cmd.Parameters.AddWithValue("@inactive", Get(f, "inactive", "N"));
                        cmd.Parameters.AddWithValue("@created", now);
                        cmd.Parameters.AddWithValue("@modified", now);
                        cmd.Parameters.AddWithValue("@user", user);

                        long key = Convert.ToInt64(cmd.ExecuteScalar());
                        tx.Commit();

                        if (jsonOut) Console.WriteLine("{\"ok\": true, \"key\": " + key + ", \"code\": \"" + JsonEsc(tag) + "\"}");
                        else Console.WriteLine("OK key=" + key + " code=" + tag);
                        return 0;
                    }
                }
                catch { try { tx.Rollback(); } catch { } throw; }
            }}
        }

        private static int ItemRead(Dictionary<string, string> f, bool jsonOut)
        {
            (string col, object val) = ResolveItemSelector(f);
            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlCommand c = new SqlCommand(
                "SELECT * FROM [dbo].[zSCP_ServiceItem] WHERE " + col + " = @v", cn))
            {
                c.Parameters.AddWithValue("@v", val);
                using (SqlDataReader r = c.ExecuteReader()) PrintReader(r, jsonOut);
            }}
            return 0;
        }

        private static int ItemUpdate(Dictionary<string, string> f, bool jsonOut)
        {
            (string col, object val) = ResolveItemSelector(f);
            string user = Get(f, "user", DefaultUser());

            // Build SET clause from supplied flags only (sparse update, mirrors form Edit semantics)
            List<string> sets = new List<string>();
            Dictionary<string, object> p = new Dictionary<string, object>();
            void Add(string flag, string col2, object value) { if (f.ContainsKey(flag)) { sets.Add(col2 + "=@" + flag); p["@" + flag] = value; } }
            Add("stock", "StockCode", Get(f, "stock", ""));
            Add("grade", "ServiceItemGradeCode", Get(f, "grade", ""));
            Add("price", "UnitPrice", ParseDecimal(f, "price", 0m));
            Add("ref", "RefNo", Get(f, "ref", ""));
            Add("desc", "[Description]", Get(f, "desc", ""));
            Add("contract", "ContractNo", Get(f, "contract", ""));
            Add("debtor", "DebtorCode", Get(f, "debtor", ""));
            Add("agent", "StaffCode", Get(f, "agent", ""));
            Add("term", "TermCode", Get(f, "term", ""));
            Add("area", "AreaCode", Get(f, "area", ""));
            Add("address", "Address1", Get(f, "address", ""));
            Add("attention", "Attention", Get(f, "attention", ""));
            Add("note", "Note", Get(f, "note", ""));
            Add("pm-type", "PMIntervalType", Get(f, "pm-type", "NONE"));
            Add("pm-interval", "PMIntervalValue", ParseInt(f, "pm-interval", 0));
            Add("pm-active", "PMActive", Get(f, "pm-active", "N"));
            Add("department", "DepartmentCode", Get(f, "department", ""));
            Add("job", "JobCode", Get(f, "job", ""));
            Add("location", "StockLocationCode", Get(f, "location", ""));
            Add("inactive", "Inactive", Get(f, "inactive", "N"));
            Add("start", "ServiceStartDate", ParseDate(f, "start", DateTime.Today));
            Add("end", "ServiceExpiryDate", ParseDate(f, "end", DateTime.Today));

            if (sets.Count == 0) { Stderr("No fields to update. Pass at least one --field=value flag."); return 2; }

            // always-on audit columns
            sets.Add("Modified=@_modified");
            sets.Add("LastModified=@_modified");
            sets.Add("ModifiedBy=@_user");

            string verPredicate = "";
            if (f.ContainsKey("if-modified"))
            {
                verPredicate = " AND LastModified = @_rv";
            }

            string sql = "UPDATE [dbo].[zSCP_ServiceItem] SET " + string.Join(", ", sets) +
                         " WHERE " + col + " = @_sel" + verPredicate;

            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlTransaction tx = cn.BeginTransaction("AtpCliItemUpdate"))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(sql, cn, tx))
                    {
                        foreach (KeyValuePair<string, object> kv in p) cmd.Parameters.AddWithValue(kv.Key, kv.Value);
                        cmd.Parameters.AddWithValue("@_modified", DateTime.UtcNow);
                        cmd.Parameters.AddWithValue("@_user", user);
                        cmd.Parameters.AddWithValue("@_sel", val);
                        if (verPredicate.Length > 0)
                            cmd.Parameters.AddWithValue("@_rv", DateTime.Parse(f["if-modified"]));

                        int rows = cmd.ExecuteNonQuery();
                        tx.Commit();

                        if (rows == 0) { Stderr(verPredicate.Length > 0 ? "Concurrency conflict — LastModified changed." : "No row matched."); return 4; }
                        if (jsonOut) Console.WriteLine("{\"ok\": true, \"rows\": " + rows + "}");
                        else Console.WriteLine("OK rows=" + rows);
                        return 0;
                    }
                }
                catch { try { tx.Rollback(); } catch { } throw; }
            }}
        }

        private static int ItemDelete(Dictionary<string, string> f)
        {
            (string col, object val) = ResolveItemSelector(f);
            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlTransaction tx = cn.BeginTransaction("AtpCliItemDelete"))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(
                        "DELETE FROM [dbo].[zSCP_ServiceItem] WHERE " + col + " = @v", cn, tx))
                    {
                        cmd.Parameters.AddWithValue("@v", val);
                        int rows = cmd.ExecuteNonQuery();
                        tx.Commit();
                        Console.WriteLine("OK rows=" + rows);
                        return rows == 0 ? 4 : 0;
                    }
                }
                catch { try { tx.Rollback(); } catch { } throw; }
            }}
        }

        private static int ItemAudit(Dictionary<string, string> f, bool jsonOut)
        {
            (string col, object val) = ResolveItemSelector(f);
            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlCommand c = new SqlCommand(
                "SELECT ServiceItemKey, ServiceItemCode, Inactive, Created, Modified, LastModified, " +
                "ISNULL(CreatedBy,'') AS CreatedBy, ISNULL(ModifiedBy,'') AS ModifiedBy " +
                "FROM [dbo].[zSCP_ServiceItem] WHERE " + col + " = @v", cn))
            {
                c.Parameters.AddWithValue("@v", val);
                using (SqlDataReader r = c.ExecuteReader()) PrintReader(r, jsonOut);
            }}
            return 0;
        }

        private static (string col, object val) ResolveItemSelector(Dictionary<string, string> f)
        {
            if (f.ContainsKey("key")) return ("ServiceItemKey", (object)long.Parse(f["key"]));
            if (f.ContainsKey("tag")) return ("ServiceItemCode", (object)f["tag"]);
            if (f.ContainsKey("code")) return ("ServiceItemCode", (object)f["code"]);
            throw new Exception("Need --tag <code> or --key <id>");
        }

        // ---------------- METER (child of item) ----------------

        private static int MeterCommand(string sub, Dictionary<string, string> f, bool jsonOut)
        {
            switch (sub)
            {
                case "add":          return MeterAdd(f, jsonOut);
                case "list":         return MeterList(f, jsonOut);
                case "delete-all":   return MeterDeleteAll(f);
                case "set-item":     return MeterSetItem(f);
                default: Stderr("Unknown meter subcommand: " + sub); return 2;
            }
        }

        /// <summary>
        /// Bind an AutoCount Item Code to a meter type for invoice generation.
        /// Usage: atp meter set-item --type MTC --item ITEM-CODE
        ///        atp meter set-item --type MTC --item ""   (to clear)
        /// </summary>
        private static int MeterSetItem(Dictionary<string, string> f)
        {
            string type = RequireFlag(f, "type");
            string item = Get(f, "item", "");
            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlCommand c = new SqlCommand(
                "UPDATE [dbo].[zSCP_MeterType] SET ACItemCode = @item WHERE MeterTypeCode = @type", cn))
            {
                c.Parameters.AddWithValue("@type", type);
                c.Parameters.AddWithValue("@item", item);
                int rows = c.ExecuteNonQuery();
                if (rows == 0) { Stderr("Meter type not found: " + type); return 4; }
                Console.WriteLine("OK " + type + " → ACItemCode=" + (string.IsNullOrEmpty(item) ? "(empty)" : item));
                return 0;
            }}
        }

        private static int MeterAdd(Dictionary<string, string> f, bool jsonOut)
        {
            string tag = RequireFlag(f, "tag");
            string type = RequireFlag(f, "type");
            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlTransaction tx = cn.BeginTransaction("AtpCliMeterAdd"))
            {
                try
                {
                    long key;
                    using (SqlCommand find = new SqlCommand(
                        "SELECT ServiceItemKey FROM [dbo].[zSCP_ServiceItem] WHERE ServiceItemCode = @c", cn, tx))
                    {
                        find.Parameters.AddWithValue("@c", tag);
                        object o = find.ExecuteScalar();
                        if (o == null) { Stderr("Service Item not found: " + tag); tx.Rollback(); return 4; }
                        key = Convert.ToInt64(o);
                    }
                    using (SqlCommand cmd = new SqlCommand(
                        "INSERT INTO [dbo].[zSCP_ServiceItemMeterType] " +
                        "(ServiceItemKey, MeterTypeCode, MeterMultiPriceCode, MinimumCharges, ChargesRate, RebateQtyInPercent, FOCQty, InitialReading) " +
                        "VALUES (@k, @t, @m, @min, @rate, @rebate, @foc, @init); SELECT CAST(SCOPE_IDENTITY() AS BIGINT);", cn, tx))
                    {
                        cmd.Parameters.AddWithValue("@k", key);
                        cmd.Parameters.AddWithValue("@t", type);
                        cmd.Parameters.AddWithValue("@m", Get(f, "multi", ""));
                        cmd.Parameters.AddWithValue("@min", ParseDecimal(f, "min", 0m));
                        cmd.Parameters.AddWithValue("@rate", ParseDecimal(f, "rate", 0m));
                        cmd.Parameters.AddWithValue("@rebate", ParseDecimal(f, "rebate", 0m));
                        cmd.Parameters.AddWithValue("@foc", ParseDecimal(f, "foc", 0m));
                        cmd.Parameters.AddWithValue("@init", ParseDecimal(f, "initial", 0m));
                        long mtKey = Convert.ToInt64(cmd.ExecuteScalar());
                        tx.Commit();
                        if (jsonOut) Console.WriteLine("{\"ok\": true, \"meterKey\": " + mtKey + ", \"itemKey\": " + key + "}");
                        else Console.WriteLine("OK meterKey=" + mtKey + " itemKey=" + key);
                        return 0;
                    }
                }
                catch { try { tx.Rollback(); } catch { } throw; }
            }}
        }

        private static int MeterList(Dictionary<string, string> f, bool jsonOut)
        {
            string tag = RequireFlag(f, "tag");
            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlCommand c = new SqlCommand(
                "SELECT smt.ServiceItemMeterTypeKey, smt.MeterTypeCode, ISNULL(mt.[Description],'') AS MeterTypeName, " +
                "ISNULL(smt.MeterMultiPriceCode,'') AS MeterMultiPriceCode, smt.ChargesRate, smt.MinimumCharges, smt.FOCQty, " +
                "smt.RebateQtyInPercent, smt.InitialReading " +
                "FROM [dbo].[zSCP_ServiceItemMeterType] smt " +
                "LEFT JOIN [dbo].[zSCP_MeterType] mt ON mt.MeterTypeCode = smt.MeterTypeCode " +
                "WHERE smt.ServiceItemKey = (SELECT ServiceItemKey FROM [dbo].[zSCP_ServiceItem] WHERE ServiceItemCode=@c) " +
                "ORDER BY smt.ServiceItemMeterTypeKey", cn))
            {
                c.Parameters.AddWithValue("@c", tag);
                using (SqlDataReader r = c.ExecuteReader()) PrintReader(r, jsonOut);
            }}
            return 0;
        }

        private static int MeterDeleteAll(Dictionary<string, string> f)
        {
            string tag = RequireFlag(f, "tag");
            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlCommand c = new SqlCommand(
                "DELETE FROM [dbo].[zSCP_ServiceItemMeterType] WHERE ServiceItemKey = " +
                "(SELECT ServiceItemKey FROM [dbo].[zSCP_ServiceItem] WHERE ServiceItemCode=@c)", cn))
            {
                c.Parameters.AddWithValue("@c", tag);
                int rows = c.ExecuteNonQuery();
                Console.WriteLine("OK rows=" + rows);
            }}
            return 0;
        }

        // ---------------- METER-TRANS (zSCP_MeterTrans) ----------------

        private static int MeterTransCommand(string sub, Dictionary<string, string> f, bool jsonOut)
        {
            switch (sub)
            {
                case "save":       return MeterTransSave(f, jsonOut);
                case "list":       return MeterTransList(f, jsonOut);
                case "count":      return MeterTransCount(f);
                case "load":       return MeterTransLoad(f, jsonOut);
                case "delete":     return MeterTransDelete(f);
                case "delete-all": return MeterTransDeleteAll(f);
                default: Stderr("Unknown meter-trans subcommand: " + sub); return 2;
            }
        }

        /// <summary>
        /// Save a meter reading for a service item's meter type.
        /// Mirrors what MeterTypeTransactionEntry_Form does for a single row.
        /// Usage: atp meter-trans save --tag SI-CODE --type MT-CODE --reading 12345 [--date 2026-04-17] [--remark TEXT]
        /// </summary>
        private static int MeterTransSave(Dictionary<string, string> f, bool jsonOut)
        {
            string tag = RequireFlag(f, "tag");
            string type = RequireFlag(f, "type");
            decimal reading = ParseDecimal(f, "reading", -1m);
            if (reading < 0m) throw new Exception("Missing required flag --reading (must be >= 0)");
            DateTime transDate = ParseDate(f, "date", DateTime.Now);
            string remark = Get(f, "remark", "CLI meter reading");

            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlTransaction tx = cn.BeginTransaction("AtpCliMeterTransSave"))
            {
                try
                {
                    // Resolve ServiceItemKey + ServiceItemMeterTypeKey
                    long siKey = 0;
                    long simtKey = 0;
                    using (SqlCommand q = new SqlCommand(
                        "SELECT TOP 1 simt.ServiceItemMeterTypeKey, simt.ServiceItemKey " +
                        "FROM [dbo].[zSCP_ServiceItemMeterType] simt " +
                        "INNER JOIN [dbo].[zSCP_ServiceItem] si ON si.ServiceItemKey = simt.ServiceItemKey " +
                        "WHERE si.ServiceItemCode = @tag AND simt.MeterTypeCode = @type", cn, tx))
                    {
                        q.Parameters.AddWithValue("@tag", tag);
                        q.Parameters.AddWithValue("@type", type);
                        DataTable dtLookup = new DataTable();
                        dtLookup.Load(q.ExecuteReader());
                        if (dtLookup.Rows.Count == 0)
                        {
                            Stderr("No matching meter type '" + type + "' for service item '" + tag + "'.");
                            tx.Rollback();
                            return 4;
                        }
                        simtKey = Convert.ToInt64(dtLookup.Rows[0][0]);
                        siKey = Convert.ToInt64(dtLookup.Rows[0][1]);
                    }

                    // Get last reading for comparison
                    decimal lastReading = 0m;
                    using (SqlCommand qlast = new SqlCommand(
                        "SELECT TOP 1 MeterTransReading FROM [dbo].[zSCP_MeterTrans] " +
                        "WHERE ServiceItemMeterTypeKey = @k ORDER BY MeterTransDate DESC, MeterTransKey DESC", cn, tx))
                    {
                        qlast.Parameters.AddWithValue("@k", simtKey);
                        object o = qlast.ExecuteScalar();
                        if (o != null && o != DBNull.Value) lastReading = Convert.ToDecimal(o);
                        else
                        {
                            // fallback to InitialReading
                            using (SqlCommand qi = new SqlCommand(
                                "SELECT InitialReading FROM [dbo].[zSCP_ServiceItemMeterType] WHERE ServiceItemMeterTypeKey = @k", cn, tx))
                            {
                                qi.Parameters.AddWithValue("@k", simtKey);
                                object oi = qi.ExecuteScalar();
                                if (oi != null && oi != DBNull.Value) lastReading = Convert.ToDecimal(oi);
                            }
                        }
                    }

                    decimal usage = reading - lastReading;
                    if (usage < 0m) usage = 0m;

                    // Insert
                    using (SqlCommand cmd = new SqlCommand(
                        "INSERT INTO [dbo].[zSCP_MeterTrans] " +
                        "(ServiceItemMeterTypeKey, ServiceItemKey, MeterTypeCode, MeterTransDate, MeterTransReading, Remark) " +
                        "OUTPUT INSERTED.MeterTransKey " +
                        "VALUES (@simtKey, @siKey, @type, @date, @reading, @remark)", cn, tx))
                    {
                        cmd.Parameters.AddWithValue("@simtKey", simtKey);
                        cmd.Parameters.AddWithValue("@siKey", siKey);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@date", transDate);
                        cmd.Parameters.AddWithValue("@reading", reading);
                        cmd.Parameters.AddWithValue("@remark", remark);
                        long newKey = Convert.ToInt64(cmd.ExecuteScalar());
                        tx.Commit();

                        if (jsonOut) Console.WriteLine("{\"ok\": true, \"meterTransKey\": " + newKey +
                            ", \"lastReading\": " + lastReading.ToString(CultureInfo.InvariantCulture) +
                            ", \"currentReading\": " + reading.ToString(CultureInfo.InvariantCulture) +
                            ", \"usage\": " + usage.ToString(CultureInfo.InvariantCulture) + "}");
                        else Console.WriteLine("OK meterTransKey=" + newKey +
                            " lastReading=" + lastReading + " currentReading=" + reading + " usage=" + usage);
                        return 0;
                    }
                }
                catch { try { tx.Rollback(); } catch { } throw; }
            }}
        }

        /// <summary>
        /// List meter transactions for a service item (or globally).
        /// Usage: atp meter-trans list --tag SI-CODE [--top N] [--type MT-CODE]
        ///        atp meter-trans list --top 20
        /// </summary>
        private static int MeterTransList(Dictionary<string, string> f, bool jsonOut)
        {
            int top = ParseInt(f, "top", 25);
            string tag = Get(f, "tag", null);
            string type = Get(f, "type", null);

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP " + top + " t.MeterTransKey, t.ServiceItemMeterTypeKey, t.ServiceItemKey, ");
            sb.Append("t.MeterTypeCode, t.MeterTransDate, t.MeterTransReading, ");
            sb.Append("ISNULL(t.SalesInvoiceDocKey,0) AS SalesInvoiceDocKey, ISNULL(t.Remark,'') AS Remark, ");
            sb.Append("si.ServiceItemCode ");
            sb.Append("FROM [dbo].[zSCP_MeterTrans] t ");
            sb.Append("LEFT JOIN [dbo].[zSCP_ServiceItem] si ON si.ServiceItemKey = t.ServiceItemKey ");

            List<string> where = new List<string>();
            if (tag != null) where.Add("si.ServiceItemCode = @tag");
            if (type != null) where.Add("t.MeterTypeCode = @type");
            if (where.Count > 0) sb.Append("WHERE " + string.Join(" AND ", where) + " ");
            sb.Append("ORDER BY t.MeterTransDate DESC, t.MeterTransKey DESC");

            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlCommand c = new SqlCommand(sb.ToString(), cn))
            {
                if (tag != null) c.Parameters.AddWithValue("@tag", tag);
                if (type != null) c.Parameters.AddWithValue("@type", type);
                using (SqlDataReader r = c.ExecuteReader()) PrintReader(r, jsonOut);
            }}
            return 0;
        }

        /// <summary>
        /// Count meter transactions, optionally filtered by service item.
        /// </summary>
        private static int MeterTransCount(Dictionary<string, string> f)
        {
            string tag = Get(f, "tag", null);
            string sql;
            if (tag != null)
                sql = "SELECT COUNT(*) FROM [dbo].[zSCP_MeterTrans] t " +
                      "INNER JOIN [dbo].[zSCP_ServiceItem] si ON si.ServiceItemKey = t.ServiceItemKey " +
                      "WHERE si.ServiceItemCode = @tag";
            else
                sql = "SELECT COUNT(*) FROM [dbo].[zSCP_MeterTrans]";

            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlCommand c = new SqlCommand(sql, cn))
            {
                if (tag != null) c.Parameters.AddWithValue("@tag", tag);
                Console.WriteLine(c.ExecuteScalar());
            }}
            return 0;
        }

        /// <summary>
        /// Load the meter grid data for a service item — same query as MeterTypeTransactionEntry_Form.LoadMeterGrid.
        /// Shows meter types with last reading info, ready for the user to enter current readings.
        /// Usage: atp meter-trans load --tag SI-CODE
        /// </summary>
        private static int MeterTransLoad(Dictionary<string, string> f, bool jsonOut)
        {
            string tag = RequireFlag(f, "tag");
            string sql =
                "SELECT simt.ServiceItemMeterTypeKey, simt.MeterTypeCode, " +
                "ISNULL(mt.[Description],'') AS MeterTypeName, " +
                "simt.MinimumCharges, simt.ChargesRate AS UnitPrice, simt.FOCQty, simt.RebateQtyInPercent, " +
                "simt.InitialReading, ISNULL(mt.ACItemCode,'') AS ACItemCode, " +
                "lr.LastTransDate, lr.LastReading " +
                "FROM [dbo].[zSCP_ServiceItemMeterType] simt " +
                "INNER JOIN [dbo].[zSCP_MeterType] mt ON mt.MeterTypeCode = simt.MeterTypeCode " +
                "INNER JOIN [dbo].[zSCP_ServiceItem] si ON si.ServiceItemKey = simt.ServiceItemKey " +
                "OUTER APPLY ( " +
                "   SELECT TOP 1 t.MeterTransDate AS LastTransDate, t.MeterTransReading AS LastReading " +
                "   FROM [dbo].[zSCP_MeterTrans] t " +
                "   WHERE t.ServiceItemMeterTypeKey = simt.ServiceItemMeterTypeKey " +
                "   ORDER BY t.MeterTransDate DESC, t.MeterTransKey DESC " +
                ") lr " +
                "WHERE si.ServiceItemCode = @tag " +
                "ORDER BY simt.MeterTypeCode";

            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlCommand c = new SqlCommand(sql, cn))
            {
                c.Parameters.AddWithValue("@tag", tag);
                using (SqlDataReader r = c.ExecuteReader()) PrintReader(r, jsonOut);
            }}
            return 0;
        }

        /// <summary>
        /// Delete a specific meter transaction by key.
        /// Usage: atp meter-trans delete --key 123
        /// </summary>
        private static int MeterTransDelete(Dictionary<string, string> f)
        {
            long key = long.Parse(RequireFlag(f, "key"));
            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlCommand c = new SqlCommand(
                "DELETE FROM [dbo].[zSCP_MeterTrans] WHERE MeterTransKey = @k", cn))
            {
                c.Parameters.AddWithValue("@k", key);
                int rows = c.ExecuteNonQuery();
                Console.WriteLine("OK rows=" + rows);
                return rows == 0 ? 4 : 0;
            }}
        }

        /// <summary>
        /// Delete all meter transactions for a service item.
        /// Usage: atp meter-trans delete-all --tag SI-CODE --confirm
        /// </summary>
        private static int MeterTransDeleteAll(Dictionary<string, string> f)
        {
            string tag = RequireFlag(f, "tag");
            bool confirm = f.ContainsKey("confirm");
            if (!confirm) { Stderr("Refusing to delete without --confirm."); return 2; }

            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlCommand c = new SqlCommand(
                "DELETE FROM [dbo].[zSCP_MeterTrans] WHERE ServiceItemKey = " +
                "(SELECT ServiceItemKey FROM [dbo].[zSCP_ServiceItem] WHERE ServiceItemCode = @tag)", cn))
            {
                c.Parameters.AddWithValue("@tag", tag);
                int rows = c.ExecuteNonQuery();
                Console.WriteLine("OK rows=" + rows);
                return 0;
            }}
        }

        // ---------------- CONTRACT ----------------

        private static int ContractCommand(string sub, Dictionary<string, string> f, bool jsonOut)
        {
            switch (sub)
            {
                case "list":   return ContractList(f, jsonOut);
                case "count":  return ContractCount();
                case "create": return ContractCreate(f, jsonOut);
                case "read":   return ContractRead(f, jsonOut);
                case "update": return ContractUpdate(f, jsonOut);
                case "delete": return ContractDelete(f);
                case "audit":  return ContractAudit(f, jsonOut);
                default: Stderr("Unknown contract subcommand: " + sub); return 2;
            }
        }

        private static int ContractList(Dictionary<string, string> f, bool jsonOut)
        {
            int top = ParseInt(f, "top", 25);
            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlCommand c = new SqlCommand(
                "SELECT TOP " + top + " ServiceContractKey, ServiceContractCode, ServiceContractTypeCode, " +
                "ISNULL(DebtorCode,'') AS DebtorCode, ISNULL([Description],'') AS [Description], " +
                "Inactive, Created, Modified FROM [dbo].[zSCP_ServiceContract] ORDER BY ServiceContractKey DESC", cn))
                using (SqlDataReader r = c.ExecuteReader()) PrintReader(r, jsonOut);
            }
            return 0;
        }

        private static int ContractCount()
        {
            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlCommand c = new SqlCommand("SELECT COUNT(*) FROM [dbo].[zSCP_ServiceContract]", cn))
                Console.WriteLine(c.ExecuteScalar());
            }
            return 0;
        }

        private static int ContractCreate(Dictionary<string, string> f, bool jsonOut)
        {
            string code = RequireFlag(f, "code");
            string user = Get(f, "user", DefaultUser());
            DateTime now = DateTime.UtcNow;

            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlTransaction tx = cn.BeginTransaction("AtpCliContractCreate"))
            {
                try
                {
                    string sql = "INSERT INTO [dbo].[zSCP_ServiceContract] " +
                        "(ServiceContractCode, ServiceContractTypeCode, ServiceContractDate, ServiceContractValue, [Description], " +
                        " ServiceStartDate, ServiceExpiryDate, DebtorCode, StaffCode, Address1, Attention, Note, Inactive, " +
                        " Created, Modified, CreatedBy, ModifiedBy) VALUES " +
                        "(@code, @type, @date, @value, @desc, @sstart, @send, @debtor, @agent, @addr, @attn, @note, @inactive, " +
                        " @created, @modified, @user, @user); SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";
                    using (SqlCommand cmd = new SqlCommand(sql, cn, tx))
                    {
                        cmd.Parameters.AddWithValue("@code", code);
                        cmd.Parameters.AddWithValue("@type", Get(f, "type", ""));
                        cmd.Parameters.AddWithValue("@date", ParseDate(f, "date", DateTime.Today));
                        cmd.Parameters.AddWithValue("@value", ParseDecimal(f, "value", 0m));
                        cmd.Parameters.AddWithValue("@desc", Get(f, "desc", "Service Contract"));
                        cmd.Parameters.AddWithValue("@sstart", ParseDate(f, "start", DateTime.Today));
                        cmd.Parameters.AddWithValue("@send", ParseDate(f, "end", DateTime.Today.AddYears(1)));
                        cmd.Parameters.AddWithValue("@debtor", Get(f, "debtor", ""));
                        cmd.Parameters.AddWithValue("@agent", Get(f, "agent", ""));
                        cmd.Parameters.AddWithValue("@addr", Get(f, "address", ""));
                        cmd.Parameters.AddWithValue("@attn", Get(f, "attention", ""));
                        cmd.Parameters.AddWithValue("@note", Get(f, "note", ""));
                        cmd.Parameters.AddWithValue("@inactive", Get(f, "inactive", "N"));
                        cmd.Parameters.AddWithValue("@created", now);
                        cmd.Parameters.AddWithValue("@modified", now);
                        cmd.Parameters.AddWithValue("@user", user);
                        long key = Convert.ToInt64(cmd.ExecuteScalar());
                        tx.Commit();
                        if (jsonOut) Console.WriteLine("{\"ok\": true, \"key\": " + key + ", \"code\": \"" + JsonEsc(code) + "\"}");
                        else Console.WriteLine("OK key=" + key + " code=" + code);
                        return 0;
                    }
                }
                catch { try { tx.Rollback(); } catch { } throw; }
            }}
        }

        private static int ContractRead(Dictionary<string, string> f, bool jsonOut)
        {
            (string col, object val) = ResolveContractSelector(f);
            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlCommand c = new SqlCommand(
                "SELECT * FROM [dbo].[zSCP_ServiceContract] WHERE " + col + " = @v", cn))
            {
                c.Parameters.AddWithValue("@v", val);
                using (SqlDataReader r = c.ExecuteReader()) PrintReader(r, jsonOut);
            }}
            return 0;
        }

        private static int ContractUpdate(Dictionary<string, string> f, bool jsonOut)
        {
            (string col, object val) = ResolveContractSelector(f);
            string user = Get(f, "user", DefaultUser());

            List<string> sets = new List<string>();
            Dictionary<string, object> p = new Dictionary<string, object>();
            void Add(string flag, string col2, object value) { if (f.ContainsKey(flag)) { sets.Add(col2 + "=@" + flag); p["@" + flag] = value; } }
            Add("type", "ServiceContractTypeCode", Get(f, "type", ""));
            Add("date", "ServiceContractDate", ParseDate(f, "date", DateTime.Today));
            Add("value", "ServiceContractValue", ParseDecimal(f, "value", 0m));
            Add("desc", "[Description]", Get(f, "desc", ""));
            Add("debtor", "DebtorCode", Get(f, "debtor", ""));
            Add("agent", "StaffCode", Get(f, "agent", ""));
            Add("address", "Address1", Get(f, "address", ""));
            Add("attention", "Attention", Get(f, "attention", ""));
            Add("note", "Note", Get(f, "note", ""));
            Add("start", "ServiceStartDate", ParseDate(f, "start", DateTime.Today));
            Add("end", "ServiceExpiryDate", ParseDate(f, "end", DateTime.Today));
            Add("inactive", "Inactive", Get(f, "inactive", "N"));

            if (sets.Count == 0) { Stderr("No fields to update."); return 2; }
            sets.Add("Modified=@_modified");
            sets.Add("LastModified=@_modified");
            sets.Add("ModifiedBy=@_user");

            string verPredicate = f.ContainsKey("if-modified") ? " AND LastModified = @_rv" : "";
            string sql = "UPDATE [dbo].[zSCP_ServiceContract] SET " + string.Join(", ", sets) +
                         " WHERE " + col + " = @_sel" + verPredicate;

            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlTransaction tx = cn.BeginTransaction("AtpCliContractUpdate"))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(sql, cn, tx))
                    {
                        foreach (KeyValuePair<string, object> kv in p) cmd.Parameters.AddWithValue(kv.Key, kv.Value);
                        cmd.Parameters.AddWithValue("@_modified", DateTime.UtcNow);
                        cmd.Parameters.AddWithValue("@_user", user);
                        cmd.Parameters.AddWithValue("@_sel", val);
                        if (verPredicate.Length > 0) cmd.Parameters.AddWithValue("@_rv", DateTime.Parse(f["if-modified"]));
                        int rows = cmd.ExecuteNonQuery();
                        tx.Commit();
                        if (rows == 0) { Stderr(verPredicate.Length > 0 ? "Concurrency conflict — LastModified changed." : "No row matched."); return 4; }
                        if (jsonOut) Console.WriteLine("{\"ok\": true, \"rows\": " + rows + "}");
                        else Console.WriteLine("OK rows=" + rows);
                        return 0;
                    }
                }
                catch { try { tx.Rollback(); } catch { } throw; }
            }}
        }

        private static int ContractDelete(Dictionary<string, string> f)
        {
            (string col, object val) = ResolveContractSelector(f);
            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlTransaction tx = cn.BeginTransaction("AtpCliContractDelete"))
            {
                try
                {
                    using (SqlCommand del = new SqlCommand(
                        "DELETE FROM [dbo].[zSCP_ServiceContract] WHERE " + col + " = @v", cn, tx))
                    {
                        del.Parameters.AddWithValue("@v", val);
                        int rows = del.ExecuteNonQuery();
                        tx.Commit();
                        Console.WriteLine("OK rows=" + rows);
                        return rows == 0 ? 4 : 0;
                    }
                }
                catch { try { tx.Rollback(); } catch { } throw; }
            }}
        }

        private static int ContractAudit(Dictionary<string, string> f, bool jsonOut)
        {
            (string col, object val) = ResolveContractSelector(f);
            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlCommand c = new SqlCommand(
                "SELECT ServiceContractKey, ServiceContractCode, Inactive, Created, Modified, LastModified, " +
                "ISNULL(CreatedBy,'') AS CreatedBy, ISNULL(ModifiedBy,'') AS ModifiedBy " +
                "FROM [dbo].[zSCP_ServiceContract] WHERE " + col + " = @v", cn))
            {
                c.Parameters.AddWithValue("@v", val);
                using (SqlDataReader r = c.ExecuteReader()) PrintReader(r, jsonOut);
            }}
            return 0;
        }

        private static (string col, object val) ResolveContractSelector(Dictionary<string, string> f)
        {
            if (f.ContainsKey("key")) return ("ServiceContractKey", (object)long.Parse(f["key"]));
            if (f.ContainsKey("code")) return ("ServiceContractCode", (object)f["code"]);
            throw new Exception("Need --code <code> or --key <id>");
        }

        // ---------------- SCHEMA / CLEANUP / SQL ----------------

        private static int SchemaCommand(string sub, Dictionary<string, string> f, bool jsonOut)
        {
            switch (sub)
            {
                case "verify":  return SchemaVerify(jsonOut);
                case "columns": return SchemaColumns(f, jsonOut);
                default: Stderr("Unknown schema subcommand: " + sub); return 2;
            }
        }

        private static int SchemaVerify(bool jsonOut)
        {
            // Confirm v1.2.0 + v1.3.0 columns are present on the master tables.
            string[] checks = new[] {
                "zSCP_ServiceItem|PMNextServiceDate",
                "zSCP_ServiceItem|CreatedBy",
                "zSCP_ServiceItem|ModifiedBy",
                "zSCP_ServiceContract|CreatedBy",
                "zSCP_ServiceContract|ModifiedBy",
            };
            using (SqlConnection cn = Db()) { cn.Open();
            int missing = 0;
            List<string> rows = new List<string>();
            foreach (string c in checks)
            {
                string[] parts = c.Split('|');
                using (SqlCommand q = new SqlCommand(
                    "SELECT CASE WHEN COL_LENGTH(@t, @c) IS NULL THEN 0 ELSE 1 END", cn))
                {
                    q.Parameters.AddWithValue("@t", "dbo." + parts[0]);
                    q.Parameters.AddWithValue("@c", parts[1]);
                    int present = Convert.ToInt32(q.ExecuteScalar());
                    rows.Add(parts[0] + "." + parts[1] + " " + (present == 1 ? "OK" : "MISSING"));
                    if (present == 0) missing++;
                }
            }
            if (jsonOut) Console.WriteLine("{\"missing\": " + missing + ", \"checks\": [" + string.Join(",", rows.ConvertAll(x => "\"" + JsonEsc(x) + "\"")) + "]}");
            else { foreach (string r2 in rows) Console.WriteLine(r2); Console.WriteLine("---"); Console.WriteLine(missing == 0 ? "ALL OK" : missing + " MISSING"); }
            return missing == 0 ? 0 : 5;
            }
        }

        private static int SchemaColumns(Dictionary<string, string> f, bool jsonOut)
        {
            string table = RequireFlag(f, "table");
            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlCommand c = new SqlCommand(
                "SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, ISNULL(CAST(COLUMN_DEFAULT AS NVARCHAR(200)),'') AS COLUMN_DEFAULT " +
                "FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @t ORDER BY ORDINAL_POSITION", cn))
            {
                c.Parameters.AddWithValue("@t", table);
                using (SqlDataReader r = c.ExecuteReader()) PrintReader(r, jsonOut);
            }}
            return 0;
        }

        private static int CleanupCommand(Dictionary<string, string> f)
        {
            string prefix = RequireFlag(f, "prefix");
            bool confirm = f.ContainsKey("confirm");
            if (!confirm) { Stderr("Refusing to delete without --confirm. Pattern: " + prefix + "*"); return 2; }

            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlTransaction tx = cn.BeginTransaction("AtpCliCleanup"))
            {
                try
                {
                    int total = 0;
                    foreach (string tbl in new[] { "zSCP_ServiceItem", "zSCP_ServiceContract" })
                    {
                        string codeCol = tbl == "zSCP_ServiceItem" ? "ServiceItemCode" : "ServiceContractCode";
                        using (SqlCommand cmd = new SqlCommand(
                            "DELETE FROM [dbo].[" + tbl + "] WHERE " + codeCol + " LIKE @p", cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@p", prefix + "%");
                            int n = cmd.ExecuteNonQuery();
                            Console.WriteLine(tbl + " rows=" + n);
                            total += n;
                        }
                    }
                    tx.Commit();
                    Console.WriteLine("Total deleted: " + total);
                    return 0;
                }
                catch { try { tx.Rollback(); } catch { } throw; }
            }}
        }

        private static int SqlCommand(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql)) { Stderr("Pass a SELECT statement as the second arg."); return 2; }
            string trimmed = sql.TrimStart();
            if (!trimmed.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase) &&
                !trimmed.StartsWith("WITH",   StringComparison.OrdinalIgnoreCase) &&
                !trimmed.StartsWith("EXEC sp_help", StringComparison.OrdinalIgnoreCase))
            { Stderr("Only SELECT / WITH / sp_help allowed in `atp sql` for safety."); return 2; }
            using (SqlConnection cn = Db()) { cn.Open();
            using (SqlCommand c = new SqlCommand(sql, cn))
            using (SqlDataReader r = c.ExecuteReader()) PrintReader(r, false);
            }
            return 0;
        }

        // ---------------- helpers ----------------

        private static SqlConnection Db()
        {
            string srv = ConfigurationManager.AppSettings["DBSetting.ServerName"];
            string db  = ConfigurationManager.AppSettings["DBSetting.DBName"];
            string usr = ConfigurationManager.AppSettings["DBSetting.User"];
            string pwd = ConfigurationManager.AppSettings["DBSetting.Password"];
            string cs  = "Data Source=" + srv + ";Initial Catalog=" + db + ";User ID=" + usr + ";Password=" + pwd + ";Encrypt=False;TrustServerCertificate=True;Connection Timeout=8";
            return new SqlConnection(cs);
        }

        private static string DefaultUser()
        {
            string u = ConfigurationManager.AppSettings["DefaultUserCode"];
            return string.IsNullOrEmpty(u) ? "ATPCLI" : u;
        }

        private static Dictionary<string, string> ParseFlags(string[] args, int start)
        {
            Dictionary<string, string> d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            for (int i = start; i < args.Length; i++)
            {
                string a = args[i];
                if (a.StartsWith("--"))
                {
                    string key = a.Substring(2);
                    int eq = key.IndexOf('=');
                    if (eq >= 0) { d[key.Substring(0, eq)] = key.Substring(eq + 1); }
                    else if (i + 1 < args.Length && !args[i + 1].StartsWith("--")) { d[key] = args[++i]; }
                    else { d[key] = "true"; } // flag-only
                }
            }
            return d;
        }

        private static string Get(Dictionary<string, string> f, string k, string fallback)
        { return f.TryGetValue(k, out string v) ? v : fallback; }
        private static int ParseInt(Dictionary<string, string> f, string k, int fallback)
        { return f.TryGetValue(k, out string v) ? int.Parse(v, CultureInfo.InvariantCulture) : fallback; }
        private static decimal ParseDecimal(Dictionary<string, string> f, string k, decimal fallback)
        { return f.TryGetValue(k, out string v) ? decimal.Parse(v, CultureInfo.InvariantCulture) : fallback; }
        private static DateTime ParseDate(Dictionary<string, string> f, string k, DateTime fallback)
        { return f.TryGetValue(k, out string v) ? DateTime.Parse(v, CultureInfo.InvariantCulture) : fallback; }
        private static string RequireFlag(Dictionary<string, string> f, string k)
        { if (!f.TryGetValue(k, out string v)) throw new Exception("Missing required flag --" + k); return v; }

        private static void PrintReader(SqlDataReader r, bool jsonOut)
        {
            if (jsonOut)
            {
                Console.Write("[");
                bool first = true;
                while (r.Read())
                {
                    if (!first) Console.Write(",");
                    Console.Write("{");
                    for (int i = 0; i < r.FieldCount; i++)
                    {
                        if (i > 0) Console.Write(",");
                        object v = r.GetValue(i);
                        Console.Write("\"" + JsonEsc(r.GetName(i)) + "\":");
                        if (v == DBNull.Value) Console.Write("null");
                        else if (v is bool b) Console.Write(b ? "true" : "false");
                        else if (v is DateTime dt) Console.Write("\"" + dt.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture) + "\"");
                        else if (v is decimal || v is double || v is float || v is int || v is long || v is short || v is byte)
                            Console.Write(Convert.ToString(v, CultureInfo.InvariantCulture));
                        else Console.Write("\"" + JsonEsc(Convert.ToString(v, CultureInfo.InvariantCulture)) + "\"");
                    }
                    Console.Write("}");
                    first = false;
                }
                Console.WriteLine("]");
            }
            else
            {
                if (r.FieldCount == 0) { Console.WriteLine("(no columns)"); return; }
                int rows = 0;
                while (r.Read())
                {
                    rows++;
                    Console.WriteLine("─── row " + rows + " ───");
                    for (int i = 0; i < r.FieldCount; i++)
                    {
                        object v = r.GetValue(i);
                        string s = v == DBNull.Value ? "(null)" : Convert.ToString(v, CultureInfo.InvariantCulture);
                        Console.WriteLine(r.GetName(i).PadRight(30) + " " + s);
                    }
                }
                if (rows == 0) Console.WriteLine("(no rows)");
            }
        }

        private static string JsonEsc(string s)
        {
            if (s == null) return "";
            StringBuilder sb = new StringBuilder(s.Length);
            foreach (char ch in s)
            {
                switch (ch)
                {
                    case '\\': sb.Append("\\\\"); break;
                    case '"':  sb.Append("\\\""); break;
                    case '\n': sb.Append("\\n");  break;
                    case '\r': sb.Append("\\r");  break;
                    case '\t': sb.Append("\\t");  break;
                    default:   if (ch < 0x20) sb.Append("\\u" + ((int)ch).ToString("X4")); else sb.Append(ch); break;
                }
            }
            return sb.ToString();
        }

        private static bool IsHelp(string a)
        { return a == "-h" || a == "--help" || a == "/?" || a == "help"; }

        private static void Stderr(string s) { Console.Error.WriteLine(s); }

        private static void PrintHelp()
        {
            Console.WriteLine(@"atp — ATP plugin CRUD CLI

Usage:
  atp <noun> <verb> [--flags...] [--json]

Item (zSCP_ServiceItem):
  atp item list   [--top N] [--like PATTERN]
  atp item count
  atp item create --tag CODE [--stock X] [--debtor X] [--desc TEXT]
                  [--agent X] [--term X] [--area X] [--ref X]
                  [--department X] [--job X] [--location X]
                  [--purchase-date YYYY-MM-DD] [--start ...] [--end ...]
                  [--price N] [--inactive Y|N] [--user CODE]
  atp item read   --tag CODE | --key N
  atp item update --tag CODE | --key N  --<field> VALUE ...
                  [--if-modified ""yyyy-MM-dd HH:mm:ss.fff""]
  atp item delete --tag CODE | --key N
  atp item audit  --tag CODE | --key N

Meter (zSCP_ServiceItemMeterType, child of item):
  atp meter add        --tag CODE --type MTC --rate N --min N
                       [--multi MMP] [--foc N] [--rebate N] [--initial N]
  atp meter list       --tag CODE
  atp meter delete-all --tag CODE
  atp meter set-item   --type MTC --item ITEM-CODE  # bind AC Item for invoicing

Meter Trans (zSCP_MeterTrans — meter reading transactions):
  atp meter-trans save       --tag CODE --type MTC --reading N
                             [--date YYYY-MM-DD] [--remark TEXT]
  atp meter-trans list       [--tag CODE] [--type MTC] [--top N]
  atp meter-trans count      [--tag CODE]
  atp meter-trans load       --tag CODE          # shows meter grid (like the form)
  atp meter-trans delete     --key N
  atp meter-trans delete-all --tag CODE --confirm

Contract (zSCP_ServiceContract):
  atp contract list   [--top N]
  atp contract count
  atp contract create --code CODE [--type X] [--debtor X] [--desc TEXT]
                      [--start ...] [--end ...] [--value N] [--user CODE]
  atp contract read   --code CODE | --key N
  atp contract update --code CODE | --key N  --<field> VALUE ...
  atp contract delete --code CODE | --key N
  atp contract audit  --code CODE | --key N

Schema:
  atp schema verify              # confirm v1.2.0 + v1.3.0 audit cols present
  atp schema columns --table T   # describe table

Cleanup (test data only):
  atp cleanup --prefix SI-TEST- --confirm

Raw SQL (SELECT-only safety filter):
  atp sql ""SELECT TOP 5 ServiceItemCode, Modified FROM zSCP_ServiceItem ORDER BY Modified DESC""

Output:
  Add --json to any command that returns rows or success status for machine-readable output.
  Exit codes: 0 OK, 1 error, 2 bad args, 3 SQL error, 4 not found / no rows, 5 schema mismatch.
");
        }
    }
}
