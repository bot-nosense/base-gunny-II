﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDataProvider.Data
{
    public class DataObject
    {
        protected bool _isDirty;
        public bool IsDirty 
        { 
            get
            {
                return _isDirty;
            }
            set
            {
                _isDirty = value;
            }
        }
    }
}
