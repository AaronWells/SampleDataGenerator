namespace edfi.sdg.test.database
{
    using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class functions : SqlDatabaseTestClass
    {

        public functions()
        {
            this.InitializeComponent();
        }

        [TestInitialize()]
        public void TestInitialize()
        {
            base.InitializeTest();
        }
        [TestCleanup()]
        public void TestCleanup()
        {
            base.CleanupTest();
        }

        [TestMethod()]
        public void fnBase36()
        {
            SqlDatabaseTestActions testActions = this.fnBase36Data;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            // Execute the test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
            SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            // Execute the post-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
            SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
        }


        #region Designer support code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction fnBase36_TestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(functions));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition BasicTest;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition EdgeCase1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition EdgeCase2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition BigNumber;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition NegativeNumber;
            this.fnBase36Data = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            fnBase36_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            BasicTest = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            EdgeCase1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            EdgeCase2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            BigNumber = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            NegativeNumber = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            // 
            // fnBase36_TestAction
            // 
            fnBase36_TestAction.Conditions.Add(BasicTest);
            fnBase36_TestAction.Conditions.Add(EdgeCase1);
            fnBase36_TestAction.Conditions.Add(EdgeCase2);
            fnBase36_TestAction.Conditions.Add(BigNumber);
            fnBase36_TestAction.Conditions.Add(NegativeNumber);
            resources.ApplyResources(fnBase36_TestAction, "fnBase36_TestAction");
            // 
            // BasicTest
            // 
            BasicTest.ColumnNumber = 1;
            BasicTest.Enabled = true;
            BasicTest.ExpectedValue = "000000PA8W";
            BasicTest.Name = "BasicTest";
            BasicTest.NullExpected = false;
            BasicTest.ResultSet = 1;
            BasicTest.RowNumber = 1;
            // 
            // EdgeCase1
            // 
            EdgeCase1.ColumnNumber = 2;
            EdgeCase1.Enabled = true;
            EdgeCase1.ExpectedValue = "000000010O";
            EdgeCase1.Name = "EdgeCase1";
            EdgeCase1.NullExpected = false;
            EdgeCase1.ResultSet = 1;
            EdgeCase1.RowNumber = 1;
            // 
            // EdgeCase2
            // 
            EdgeCase2.ColumnNumber = 3;
            EdgeCase2.Enabled = true;
            EdgeCase2.ExpectedValue = "0000000004";
            EdgeCase2.Name = "EdgeCase2";
            EdgeCase2.NullExpected = false;
            EdgeCase2.ResultSet = 1;
            EdgeCase2.RowNumber = 1;
            // 
            // BigNumber
            // 
            BigNumber.ColumnNumber = 4;
            BigNumber.Enabled = true;
            BigNumber.ExpectedValue = "RLS1YK9RF5";
            BigNumber.Name = "BigNumber";
            BigNumber.NullExpected = false;
            BigNumber.ResultSet = 1;
            BigNumber.RowNumber = 1;
            // 
            // NegativeNumber
            // 
            NegativeNumber.ColumnNumber = 5;
            NegativeNumber.Enabled = true;
            NegativeNumber.ExpectedValue = "0";
            NegativeNumber.Name = "NegativeNumber";
            NegativeNumber.NullExpected = false;
            NegativeNumber.ResultSet = 1;
            NegativeNumber.RowNumber = 1;
            // 
            // fnBase36Data
            // 
            this.fnBase36Data.PosttestAction = null;
            this.fnBase36Data.PretestAction = null;
            this.fnBase36Data.TestAction = fnBase36_TestAction;
        }

        #endregion


        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        #endregion

        private SqlDatabaseTestActions fnBase36Data;
    }
}
