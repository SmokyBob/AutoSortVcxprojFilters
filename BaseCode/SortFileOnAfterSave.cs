﻿using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace BaseCode
{
    internal class SortFileOnAfterSave : IVsRunningDocTableEvents
    {
        IVsRunningDocumentTable m_IVsRunningDocumentTable;

        public SortFileOnAfterSave(IVsRunningDocumentTable i_IVsRunningDocumentTable)
        {
            m_IVsRunningDocumentTable = i_IVsRunningDocumentTable;
        }

        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterSave(uint docCookie)
        {
            uint pgrfRDTFlags, pdwReadLocks, pdwEditLocks, pitemid;
            IVsHierarchy ppHier;
            IntPtr ppunkDocData;

            string fullDocumentName;

            // Get document name
            m_IVsRunningDocumentTable.GetDocumentInfo(
                docCookie,
                out pgrfRDTFlags,
                out pdwReadLocks,
                out pdwEditLocks,
                out fullDocumentName,
                out ppHier,
                out pitemid,
                out ppunkDocData);
            //Sort only the sqlProj
            if (fullDocumentName.EndsWith(@"sqlproj"))
            {
                SqlProjSorter.Sort(fullDocumentName);
            }
            //Sort edmx files
            if (fullDocumentName.EndsWith(@"edmx"))
            {
                EdmxSorter.Sort(fullDocumentName);
            }

            return VSConstants.S_OK;
        }

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }
    }
}
