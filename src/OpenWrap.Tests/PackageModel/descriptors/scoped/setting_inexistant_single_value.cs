﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenWrap.PackageModel;
namespace OpenWrap.PackageModel.descriptors.scoped
{

    public class setting_inexistant_single_value
        : contexts.scoped_descriptor
    {
        public setting_inexistant_single_value()
        {
            given_descriptor("name: one-ring");
            given_scoped_descriptor();

            ScopedDescriptor.Anchored = true;

            when_writing();
        }

        [Test]
        public void value_is_set_in_scoped()
        {
            scoped_descriptor_should_be("anchored: true");
        }
        [Test]
        public void value_is_not_set_in_default()
        {
            descriptor_should_be("name: one-ring");
        }
    }
}
