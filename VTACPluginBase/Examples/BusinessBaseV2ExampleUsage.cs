using System;
using System.Threading.Tasks;
using AutoCount.Data;
using VTACPluginBase.Classes.BusinessBase;
using VTACPluginBase.Classes.DI;

namespace VTACPluginBase.Examples
{
    /// <summary>
    /// Example implementation of BusinessBaseV2_0_0_Cls
    /// Demonstrates how to use the new V2.0.0 features while preserving original functionality
    /// </summary>
    public class SampleBusinessEntityV2 : BusinessBaseV2_0_0_Cls
    {
        #region " Sample Fields "
        public string CustomerCode = "";
        public string CustomerName = "";
        public decimal Amount = 0;
        public DateTime TransactionDate = DateTime.Today;
        public string Status = "NEW";
        #endregion " Sample Fields "

        #region " Required Abstract Properties Implementation "
        public override string Name => "Sample Business Entity V2.0.0";

        protected override string dbTableNameMaster => "SampleBusinessEntity";
        protected override string dbQueryNameMaster => "v_SampleBusinessEntity";

        public override long PrimaryKey 
        { 
            get => GetPrimaryKeyValue(); 
            set => SetPrimaryKeyValue(value); 
        }

        protected override string SysConfigDocNoFormat => "SampleDocNoFormat";
        protected override string DocNoFormat => "SE-{yyMM}-{000000}";

        [MyAttributes(IsMainChildClass = true)]
        public override SampleDetailEntityV2 Detail { get; } = new SampleDetailEntityV2();
        #endregion " Required Abstract Properties Implementation "

        #region " Constructor "
        public SampleBusinessEntityV2()
        {
            // Constructor automatically calls base constructor which:
            // 1. Initializes DI dependencies
            // 2. Sets up data structures
            // 3. Collects child details
        }
        #endregion " Constructor "

        #region " Private Fields for Primary Key "
        private long _primaryKey = -1;

        private long GetPrimaryKeyValue() => _primaryKey;
        private void SetPrimaryKeyValue(long value) => _primaryKey = value;
        #endregion " Private Fields for Primary Key "

        #region " Custom Validation "
        protected override bool ValidateEntity()
        {
            // Call base validation first
            if (!base.ValidateEntity())
                return false;

            // Custom validation logic
            if (string.IsNullOrWhiteSpace(CustomerCode))
                ValidationErrors.Add("Customer code is required");

            if (string.IsNullOrWhiteSpace(CustomerName))
                ValidationErrors.Add("Customer name is required");

            if (Amount <= 0)
                ValidationErrors.Add("Amount must be greater than zero");

            return !HasValidationErrors;
        }
        #endregion " Custom Validation "

        #region " Custom Save Logic "
        protected override string BeforeSave(DBSetting dbSetting)
        {
            // Custom logic before save
            if (IsNewRecord)
            {
                // Generate document number if new
                if (string.IsNullOrEmpty(No))
                    No = GenerateDocumentNumber();
            }

            return ""; // Return empty string if no errors
        }

        protected override string AfterSave(DBSetting dbSetting)
        {
            // Custom logic after save
            Logger.Write($"{Name}.AfterSave", new Exception($"Successfully saved entity: {No}"));

            return ""; // Return empty string if no errors
        }

        private string GenerateDocumentNumber()
        {
            return $"SE{DateTime.Now:yyyyMMdd}{DateTime.Now.Ticks % 10000:D4}";
        }
        #endregion " Custom Save Logic "
    }

    /// <summary>
    /// Example implementation of BusinessBaseDTLV2_0_0_Cls
    /// </summary>
    public class SampleDetailEntityV2 : BusinessBaseDTLV2_0_0_Cls
    {
        #region " Sample Detail Fields "
        public string ItemCode = "";
        public string ItemDescription = "";
        public decimal Quantity = 0;
        public decimal UnitPrice = 0;
        public decimal LineTotal = 0;
        #endregion " Sample Detail Fields "

        #region " Required Abstract Properties Implementation "
        public override string Name => "Sample Detail Entity V2.0.0";

        public override long PrimaryKey 
        { 
            get => GetPrimaryKeyValue(); 
            set => SetPrimaryKeyValue(value); 
        }

        public override string DetailTableName => "SampleBusinessEntityDetail";

        public override string DetailTableQryName => "v_SampleBusinessEntityDetail";
        #endregion " Required Abstract Properties Implementation "

        #region " Private Primary Key Field "
        private long _dtlKey = -1;

        private long GetPrimaryKeyValue() => _dtlKey;
        private void SetPrimaryKeyValue(long value) => _dtlKey = value;
        #endregion " Private Primary Key Field "

        #region " Custom Validation "
        protected override bool ValidateEntity()
        {
            // Call base validation first
            if (!base.ValidateEntity())
                return false;

            // Custom validation logic
            if (string.IsNullOrWhiteSpace(ItemCode))
                ValidationErrors.Add("Item code is required");

            if (Quantity <= 0)
                ValidationErrors.Add("Quantity must be greater than zero");

            if (UnitPrice < 0)
                ValidationErrors.Add("Unit price cannot be negative");

            // Calculate line total
            LineTotal = Quantity * UnitPrice;

            return !HasValidationErrors;
        }
        #endregion " Custom Validation "
    }

    /// <summary>
    /// Example usage of the new V2.0.0 Business Base classes
    /// </summary>
    public static class BusinessBaseV2ExampleUsage
    {
        /// <summary>
        /// Example: Initialize DI Container
        /// </summary>
        public static void InitializeDI()
        {
            // Option 1: Use Simple Container (recommended for .NET Framework 4.8)
            DIConfiguration.ConfigureSimpleContainer(container =>
            {
                // Register custom services
                container.RegisterSingleton<ICustomService>(new CustomServiceImplementation());
                container.RegisterTransient<IDataProcessor>(() => new DataProcessor());
            });

            // Option 2: Ensure DI is initialized (uses defaults if not configured)
            DIConfiguration.EnsureInitialized();

            Console.WriteLine($"DI Container configured: {DIConfiguration.GetCurrentContainerType()}");
        }

        /// <summary>
        /// Example: Traditional synchronous usage (preserves original behavior)
        /// </summary>
        public static void TraditionalSyncUsage()
        {
            // Ensure DI is initialized
            DIConfiguration.EnsureInitialized();

            var entity = new SampleBusinessEntityV2
            {
                CustomerCode = "CUST001",
                CustomerName = "Test Customer",
                Amount = 1500.00m,
                TransactionDate = DateTime.Today,
                DocNo = "SE-" + DateTime.Now.ToString("yyyyMMdd") + "-001"
            };

            // Add detail
            var detail = new SampleDetailEntityV2
            {
                ItemCode = "ITEM001",
                ItemDescription = "Sample Item",
                Quantity = 2,
                UnitPrice = 750.00m
            };
            entity.Detail.AddNewDetail(detail);

            try
            {
                // Traditional synchronous save (same as original)
                string saveResult = entity.Save();
                if (string.IsNullOrEmpty(saveResult))
                {
                    Console.WriteLine($"Entity saved successfully. PrimaryKey: {entity.PrimaryKey}");
                }
                else
                {
                    Console.WriteLine($"Save failed: {saveResult}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Example: Modern async usage (new feature)
        /// </summary>
        public static async Task ModernAsyncUsage()
        {
            // Ensure DI is initialized
            DIConfiguration.EnsureInitialized();

            var entity = new SampleBusinessEntityV2
            {
                CustomerCode = "CUST002",
                CustomerName = "Async Customer",
                Amount = 2500.00m,
                TransactionDate = DateTime.Today,
                DocNo = "SE-" + DateTime.Now.ToString("yyyyMMdd") + "-002"
            };

            try
            {
                // Modern async save with operation result
                var result = await entity.SaveAsync();
                
                if (result.IsSuccess)
                {
                    Console.WriteLine($"Entity saved successfully: {result.Message}");
                    Console.WriteLine($"PrimaryKey: {entity.PrimaryKey}");
                }
                else
                {
                    Console.WriteLine($"Save failed: {result.Message}");
                    if (result.Exception != null)
                    {
                        Console.WriteLine($"Exception: {result.Exception.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Example: Load and modify existing entity
        /// </summary>
        public static async Task LoadAndModifyExample()
        {
            DIConfiguration.EnsureInitialized();

            var entity = new SampleBusinessEntityV2();

            try
            {
                // Load existing entity
                entity.Load("123"); // DocKey = 123

                if (!entity.IsNewRecord)
                {
                    // Modify entity
                    entity.CustomerName = "Updated Customer Name";
                    entity.Amount += 100.00m;

                    using (var dbSetting = new DBSetting("YourConnectionString"))
                    {
                        // Save changes (both sync and async available)
                        var result = await entity.SaveAsync(dbSetting);
                        
                        Console.WriteLine(result.IsSuccess 
                            ? "Entity updated successfully" 
                            : $"Update failed: {result.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Entity not found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Example: Validation and error handling
        /// </summary>
        public static void ValidationExample()
        {
            DIConfiguration.EnsureInitialized();

            var entity = new SampleBusinessEntityV2
            {
                // Intentionally leave required fields empty to trigger validation
                CustomerCode = "", // This will cause validation error
                CustomerName = "", // This will cause validation error
                Amount = -100      // This will cause validation error
            };

            try
            {
                using (var dbSetting = new DBSetting("YourConnectionString"))
                {
                    string saveResult = entity.Save(dbSetting);
                    
                    if (!string.IsNullOrEmpty(saveResult))
                    {
                        Console.WriteLine($"Validation failed: {saveResult}");
                        
                        // Show individual validation errors
                        if (entity.HasValidationErrors)
                        {
                            Console.WriteLine("Validation Errors:");
                            foreach (var error in entity.ValidationErrors)
                            {
                                Console.WriteLine($"- {error}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Example: Using DI services in business logic
        /// </summary>
        public static void DIServiceUsageExample()
        {
            DIConfiguration.EnsureInitialized();

            var entity = new SampleBusinessEntityV2();

            // Access DI services through ServiceLocator
            var serviceLocator = ServiceLocatorProvider.Current;
            var customService = serviceLocator.GetService<ICustomService>();
            
            if (customService != null)
            {
                customService.ProcessEntity(entity);
                Console.WriteLine("Entity processed using DI service");
            }
            else
            {
                Console.WriteLine("Custom service not registered");
            }
        }

        #region " Sample Service Interfaces "
        public interface ICustomService
        {
            void ProcessEntity(BusinessBaseV2_0_0_Cls entity);
        }

        public interface IDataProcessor
        {
            void ProcessData(object data);
        }

        public class CustomServiceImplementation : ICustomService
        {
            public void ProcessEntity(BusinessBaseV2_0_0_Cls entity)
            {
                // Custom processing logic
                Console.WriteLine($"Processing entity: {entity.Name}");
            }
        }

        public class DataProcessor : IDataProcessor
        {
            public void ProcessData(object data)
            {
                // Data processing logic
                Console.WriteLine($"Processing data: {data}");
            }
        }
        #endregion " Sample Service Interfaces "
    }
}
