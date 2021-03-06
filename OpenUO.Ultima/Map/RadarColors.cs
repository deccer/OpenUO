﻿#region License Header

// /***************************************************************************
//  *   Copyright (c) 2011 OpenUO Software Team.
//  *   All Right Reserved.
//  *
//  *   RadarColors.cs
//  *
//  *   This program is free software; you can redistribute it and/or modify
//  *   it under the terms of the GNU General Public License as published by
//  *   the Free Software Foundation; either version 3 of the License, or
//  *   (at your option) any later version.
//  ***************************************************************************/

#endregion

#region Usings

using System.IO;

#endregion

namespace OpenUO.Ultima
{
    public unsafe class RadarColors
    {
        private readonly InstallLocation _install;
        private short[] _colors;

        public RadarColors(InstallLocation install)
        {
            _install = install;
            Load();
        }

        public int Length
        {
            get { return _colors.Length; }
        }

        public short this[int index]
        {
            get { return _colors[index]; }
            set { _colors[index] = value; }
        }

        private void Load()
        {
            var path = _install.GetPath("radarcol.mul");

            if(!File.Exists(path))
            {
                return;
            }

            var file = new FileInfo(path);
            _colors = new short[file.Length / 2];

            fixed(short* ptr = _colors)
            {
                using(var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    NativeMethods._lread(fs.SafeFileHandle, ptr, 0x10000);
                }
            }
        }

        private void Save()
        {
            var path = _install.GetPath("radarcol.mul");

            fixed(short* ptr = _colors)
            {
                using(var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    NativeMethods._lwrite(fs.SafeFileHandle, ptr, _colors.Length);
                }
            }
        }
    }
}