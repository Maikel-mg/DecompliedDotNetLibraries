﻿namespace System.ServiceModel.Channels
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [TypeForwardedFrom("System.WorkflowServices, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public interface IContextManager
    {
        IDictionary<string, string> GetContext();
        void SetContext(IDictionary<string, string> context);

        bool Enabled { get; set; }
    }
}

