﻿//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters
{
    /// <summary>
    /// Provides an implementation for <see cref="PriorityFilterData"/> that
    /// processes policy overrides, performing appropriate logging of 
    /// policy processing errors.
    /// </summary>
    public class PriorityFilterDataManageabilityProvider
        : ConfigurationElementManageabilityProviderBase<PriorityFilterData>
    {
        /// <summary>
        /// The name of the maximum property.
        /// </summary>
        public const String MaximumPriorityPropertyName = "maximumPriority";

        /// <summary>
        /// The name of the minimum property.
        /// </summary>
        public const String MinimumPriorityPropertyName = "minimumPriority";

        /// <summary>
        /// Initialize an instance of the <see cref="PriorityFilterDataManageabilityProvider"/> class.
        /// </summary>
        public PriorityFilterDataManageabilityProvider()
        { }

        /// <summary>
        /// Gets the template for the name of the policy associated to the object.
        /// </summary>
        /// <remarks>
        /// Elements that override 
        /// <see cref="ConfigurationElementManageabilityProviderBase{T}.AddAdministrativeTemplateDirectives(AdmContentBuilder, T, IConfigurationSource, String)"/>
        /// to avoid creating a policy must still override this property.
        /// </remarks>
        protected override string ElementPolicyNameTemplate
        {
            get { return Resources.FilterPolicyNameTemplate; }
        }

        /// <summary>
        /// Adds the ADM parts that represent the properties of
        /// a specific instance of the configuration element type managed by the receiver.
        /// </summary>
        /// <param name="contentBuilder">The <see cref="AdmContentBuilder"/> to which the Adm instructions are to be appended.</param>
        /// <param name="configurationObject">The configuration object instance.</param>
        /// <param name="configurationSource">The configuration source from where to get additional configuration
        /// information, if necessary.</param>
        /// <param name="elementPolicyKeyName">The key for the element's policies.</param>
        /// <remarks>
        /// Subclasses managing objects that must not create a policy will likely need to include the elements' keys when creating the parts.
        /// </remarks>
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
                                                                      PriorityFilterData configurationObject,
                                                                      IConfigurationSource configurationSource,
                                                                      String elementPolicyKeyName)
        {
            contentBuilder.AddNumericPart(Resources.PriorityFilterMaximumPriorityPartName,
                                          null,
                                          MaximumPriorityPropertyName,
                                          configurationObject.MaximumPriority,
                                          0,
                                          999999999);

            contentBuilder.AddNumericPart(Resources.PriorityFilterMinimumPriorityPartName,
                                          null,
                                          MinimumPriorityPropertyName,
                                          configurationObject.MinimumPriority,
                                          0,
                                          999999999);
        }

        /// <summary>
        /// Overrides the <paramref name="configurationObject"/>'s properties with the Group Policy values from the 
        /// registry.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="policyKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides for the 
        /// configuration element.</param>
        /// <remarks>Subclasses implementing this method must retrieve all the override values from the registry
        /// before making modifications to the <paramref name="configurationObject"/> so any error retrieving
        /// the override values will cancel policy processing.</remarks>
        protected override void OverrideWithGroupPolicies(PriorityFilterData configurationObject,
                                                          IRegistryKey policyKey)
        {
            int? minimumPriorityOverride = policyKey.GetIntValue(MinimumPriorityPropertyName);
            int? maximumPriorityOverride = policyKey.GetIntValue(MaximumPriorityPropertyName);

            configurationObject.MinimumPriority = minimumPriorityOverride.Value;
            configurationObject.MaximumPriority = maximumPriorityOverride.Value;
        }
    }
}
