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
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.Azure.Commands.Network;
using Microsoft.Azure.Commands.Network.NetworkSecurityGroup.Model;
using Microsoft.WindowsAzure.Commands.Common;
using Microsoft.WindowsAzure.Commands.ServiceManagement.Properties;
using Microsoft.Azure.Common.Extensions;

namespace Microsoft.WindowsAzure.Commands.ServiceManagement.IaaS
{
    [Cmdlet(VerbsCommon.Get, NetworkSecurityGroupConfig), OutputType(typeof(IEnumerable<INetworkSecurityGroup>))]
    public class GetAzureNetworkSecurityGroupConfigCommand : VirtualMachineConfigurationCmdletBase
    {
        [Parameter(Mandatory = false)]
        [ValidateNotNullOrEmpty]
        public SwitchParameter Detailed { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            var networkConfiguration = GetNetworkConfiguration();
            if (networkConfiguration == null)
            {
                throw new ArgumentOutOfRangeException(Resources.NetworkConfigurationNotFoundOnPersistentVM);
            }

            string networkSecurityGroupName = networkConfiguration.NetworkSecurityGroup;
            if (string.IsNullOrEmpty(networkSecurityGroupName))
            {
                WriteWarningWithTimestamp(string.Format(Resources.WarningVmIsNotDirectlyAssociatedWithNetworkSecurityGroup, this.VM.GetInstance().RoleName));
            }

            else
            {
                var networkClient = new NetworkClient(AzureSession.CurrentContext.Subscription, CommandRuntime);
                INetworkSecurityGroup networkSecurityGroup = networkClient.GetNetworkSecurityGroup(networkSecurityGroupName, Detailed);

                WriteObject(networkSecurityGroup, true);
            }
        }
    }
}
