﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System;
using Microsoft.Azure.Commands.Automation.Cmdlet;
using Microsoft.Azure.Commands.Automation.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Commands.Common.Test.Mocks;
using Microsoft.WindowsAzure.Commands.Test.Utilities.Common;
using Moq;

namespace Microsoft.Azure.Commands.Automation.Test.UnitTests
{
    [TestClass]
    public class SetAzureAutomationScheduleTest : TestBase
    {
        private Mock<IAutomationClient> mockAutomationClient;

        private MockCommandRuntime mockCommandRuntime;

        private SetAzureAutomationSchedule cmdlet;

        [TestInitialize]
        public void SetupTest()
        {
            this.mockAutomationClient = new Mock<IAutomationClient>();
            this.mockCommandRuntime = new MockCommandRuntime();
            this.cmdlet = new SetAzureAutomationSchedule
                              {
                                  AutomationClient = this.mockAutomationClient.Object,
                                  CommandRuntime = this.mockCommandRuntime
                              };
        }

        [TestMethod]
        public void SetAzureAutomationScheduleByIdSuccessfull()
        {
            // Setup
            string accountName = "automation";
            string description = "desc";
            bool? isEnabled = true;
            var scheduleId = new Guid();

            this.mockAutomationClient.Setup(f => f.UpdateSchedule(accountName, scheduleId, isEnabled, description));

            // Test
            this.cmdlet.AutomationAccountName = accountName;
            this.cmdlet.Id = scheduleId;
            this.cmdlet.IsEnabled = isEnabled;
            this.cmdlet.Description = description;
            this.cmdlet.ExecuteCmdlet();

            // Assert
            this.mockAutomationClient.Verify(f => f.UpdateSchedule(accountName, scheduleId, isEnabled, description), Times.Once());
        }

        [TestMethod]
        public void SetAzureAutomationScheduleByNameSuccessfull()
        {
            // Setup
            string accountName = "automation";
            string scheduleName = "schedule";
            string description = "desc";

            this.mockAutomationClient.Setup(f => f.UpdateSchedule(accountName, scheduleName, null, description));

            // Test
            this.cmdlet.AutomationAccountName = accountName;
            this.cmdlet.Name = scheduleName;
            this.cmdlet.Description = description;
            this.cmdlet.ExecuteCmdlet();

            // Assert
            this.mockAutomationClient.Verify(f => f.UpdateSchedule(accountName, scheduleName, null, description), Times.Once());
        }
    }
}
