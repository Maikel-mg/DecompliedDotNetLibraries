﻿namespace System.Web.UI
{
    using System;
    using System.Collections;

    public interface IHierarchicalEnumerable : IEnumerable
    {
        IHierarchyData GetHierarchyData(object enumeratedItem);
    }
}

