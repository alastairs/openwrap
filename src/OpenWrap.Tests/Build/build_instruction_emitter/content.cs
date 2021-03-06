﻿using NUnit.Framework;
using OpenWrap.Tests.Build.build_instruction_emitter_specs.contexts;

namespace OpenWrap.Build.build_instruction_emitter
{
    public class content : msbuild_emitter
    {
        public content()
        {
            given_export_name("bin-net35");
            given_content_file("one-ring.cs");
            when_generating_instructions();
        }

        [Test]
        public void exported()
        {
            should_have_file("bin-net35", "one-ring.cs");
        }
    }
}