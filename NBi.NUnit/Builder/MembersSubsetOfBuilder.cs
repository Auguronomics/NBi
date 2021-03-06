﻿using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class MembersSubsetOfBuilder : AbstractMembersBuilder
    {
        protected SubsetOfXml ConstraintXml { get; set; }

        public MembersSubsetOfBuilder() : base()
        {
        }

        internal MembersSubsetOfBuilder(DiscoveryRequestFactory factory)
            : base(factory)
        {
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is SubsetOfXml))
                throw new ArgumentException("Constraint must be a 'MembersSubsetOfBuilder'");

            ConstraintXml = (SubsetOfXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected NBiConstraint InstantiateConstraint(SubsetOfXml ctrXml)
        {
            NBi.NUnit.Member.SubsetOfConstraint ctr;
            if (ctrXml.Query != null)
                ctr = new NBi.NUnit.Member.SubsetOfConstraint(ctrXml.Query.GetCommand());
            else if (ctrXml.Members != null)
            {
                var disco = InstantiateMembersDiscovery(ctrXml.Members);
                ctr = new NBi.NUnit.Member.SubsetOfConstraint(disco);
            }
            else
                ctr = new NBi.NUnit.Member.SubsetOfConstraint(ctrXml.GetItems());

            //Ignore-case if requested
            if (ctrXml.IgnoreCase)
                ctr = ctr.IgnoreCase;
            return ctr;
        }

    }
}
