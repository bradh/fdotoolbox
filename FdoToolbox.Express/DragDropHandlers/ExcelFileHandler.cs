﻿#region LGPL Header
// Copyright (C) 2010, Jackie Ng
// http://code.google.com/p/fdotoolbox, jumpinjackie@gmail.com
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
// 
//
// See license.txt for more/additional licensing information
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using FdoToolbox.Base.Services.DragDropHandlers;
using FdoToolbox.Base.Services;
using FdoToolbox.Core.Feature;
using ICSharpCode.Core;

namespace FdoToolbox.Express.DragDropHandlers
{
    public class ExcelFileHandler : IDragDropHandler
    {
        public string HandlerAction
        {
            get { return "Create new ODBC (Excel) connection"; }
        }

        string[] extensions = { ".xls" };

        public string[] FileExtensions
        {
            get { return extensions; }
        }

        public void HandleDrop(string file)
        {
            IFdoConnectionManager connMgr = ServiceManager.Instance.GetService<IFdoConnectionManager>();
            NamingService namer = ServiceManager.Instance.GetService<NamingService>();
            FdoConnection conn = null;
            try
            {
#if X64
                conn = new FdoConnection("OSGeo.ODBC", "ConnectionString=\"Driver={Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)};Dbq=" + file + "\"");
#else
                conn = new FdoConnection("OSGeo.ODBC", "ConnectionString=\"Driver={Microsoft Excel Driver (*.xls)};DriverId=790;Dbq="+file+"\"");
#endif
            }
            catch (Exception ex)
            {
                LoggingService.Error("Failed to load connection", ex);
                return;
            }

            string name = namer.GetDefaultConnectionName(conn.Provider, System.IO.Path.GetFileNameWithoutExtension(file));
            connMgr.AddConnection(name, conn);
        }
    }
}
