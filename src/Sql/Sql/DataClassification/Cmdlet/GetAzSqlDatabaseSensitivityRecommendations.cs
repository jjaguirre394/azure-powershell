﻿using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Commands.Sql.Common;
using Microsoft.Azure.Commands.Sql.DataClassification.Model;
using Microsoft.Azure.Commands.Sql.DataClassification.Services;
using Microsoft.Azure.Commands.Sql.Database.Model;
using System.Management.Automation;

namespace Microsoft.Azure.Commands.Sql.DataClassification.Cmdlet
{
    [Cmdlet(
        VerbsCommon.Get,
        ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "SqlDatabaseSensitivityRecommendations"),
        OutputType(typeof(SqlDatabaseSensitivityClassificationModel))]
    public class GetAzSqlDatabaseSensitivityRecommendations : AzureSqlDatabaseCmdletBase<SqlDatabaseSensitivityClassificationModel, DataClassificationAdapter>
    {
        [Parameter(
            ParameterSetName = DataClassificationCommon.DatabaseParameterSet,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            Position = 0,
            HelpMessage = DataClassificationCommon.ResourceGroupNameHelpMessage)]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty]
        public override string ResourceGroupName { get; set; }

        [Parameter(
            ParameterSetName = DataClassificationCommon.DatabaseParameterSet,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            Position = 1,
            HelpMessage = DataClassificationCommon.ServerNameHelpMessage)]
        [ResourceNameCompleter("Microsoft.Sql/servers", "ResourceGroupName")]
        [ValidateNotNullOrEmpty]
        public override string ServerName { get; set; }

        [Parameter(
            ParameterSetName = DataClassificationCommon.DatabaseParameterSet,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            Position = 2,
            HelpMessage = DataClassificationCommon.DatabaseNameHelpMessage)]
        [ResourceNameCompleter("Microsoft.Sql/servers/databases", "ResourceGroupName", "ServerName")]
        [ValidateNotNullOrEmpty]
        public override string DatabaseName { get; set; }

        [Parameter(
            ParameterSetName = DataClassificationCommon.DatabaseObjectParameterSet,
            Mandatory = true,
            ValueFromPipeline = true,
            HelpMessage = DataClassificationCommon.SqlDatabaseObjectHelpMessage)]
        [ValidateNotNull]
        public AzureSqlDatabaseModel DatabaseObject { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = DataClassificationCommon.AsJobHelpMessage)]
        public SwitchParameter AsJob { get; set; }

        protected override SqlDatabaseSensitivityClassificationModel GetEntity()
        {
            if (DatabaseObject != null)
            {
                ResourceGroupName = DatabaseObject.ResourceGroupName;
                ServerName = DatabaseObject.ServerName;
                DatabaseName = DatabaseObject.DatabaseName;
            }

            return new SqlDatabaseSensitivityClassificationModel()
            {
                ResourceGroupName = ResourceGroupName,
                ServerName = ServerName,
                DatabaseName = DatabaseName,
                SensitivityLabels = ModelAdapter.GetRecommendedSensitivityLabels(ResourceGroupName, ServerName, DatabaseName)
            };
        }

        protected override DataClassificationAdapter InitModelAdapter()
        {
            return new DataClassificationAdapter(DefaultProfile.DefaultContext);
        }
    }
}
