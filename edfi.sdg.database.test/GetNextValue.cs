using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace edfi.sdg.test.database
{
    [TestClass()]
    public class GetNextValue : SqlDatabaseTestClass
    {

        public GetNextValue()
        {
            InitializeComponent();
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
        public void RetrieveValuesOnPopulation()
        {
            SqlDatabaseTestActions testActions = this.RetrieveValuesOnPopulationData;
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
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction RetrieveValuesOnPopulation_PretestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction RetrieveValuesOnPopulation_PosttestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetNextValue));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction RetrieveValuesOnPopulation_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition rowCountCondition1;
            this.RetrieveValuesOnPopulationData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            RetrieveValuesOnPopulation_PretestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            RetrieveValuesOnPopulation_PosttestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            RetrieveValuesOnPopulation_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            rowCountCondition1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            // 
            // RetrieveValuesOnPopulationData
            // 
            this.RetrieveValuesOnPopulationData.PosttestAction = RetrieveValuesOnPopulation_PosttestAction;
            this.RetrieveValuesOnPopulationData.PretestAction = RetrieveValuesOnPopulation_PretestAction;
            this.RetrieveValuesOnPopulationData.TestAction = RetrieveValuesOnPopulation_TestAction;
            // 
            // RetrieveValuesOnPopulation_PretestAction
            // 
            resources.ApplyResources(RetrieveValuesOnPopulation_PretestAction, "RetrieveValuesOnPopulation_PretestAction");
            // 
            // RetrieveValuesOnPopulation_PosttestAction
            // 
            resources.ApplyResources(RetrieveValuesOnPopulation_PosttestAction, "RetrieveValuesOnPopulation_PosttestAction");
            // 
            // RetrieveValuesOnPopulation_TestAction
            // 
            RetrieveValuesOnPopulation_TestAction.Conditions.Add(rowCountCondition1);
            resources.ApplyResources(RetrieveValuesOnPopulation_TestAction, "RetrieveValuesOnPopulation_TestAction");
            // 
            // rowCountCondition1
            // 
            rowCountCondition1.Enabled = true;
            rowCountCondition1.Name = "rowCountCondition1";
            rowCountCondition1.ResultSet = 1;
            rowCountCondition1.RowCount = 3;
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

        private SqlDatabaseTestActions RetrieveValuesOnPopulationData;
    }
}
