using System;
using System.Runtime.InteropServices;

namespace Lists 
{
    public unsafe struct LinkedNode 
    {
        public int Id;
        public IntPtr Names;
        public IntPtr LastNames;
        public IntPtr Email;
        public IntPtr Password;
        public LinkedNode* Next;

        public LinkedNode(int id, string names, string lastNames, string email, string password)
        {
            Id = id;
            Names = Marshal.StringToHGlobalUni(names);
            LastNames = Marshal.StringToHGlobalUni(lastNames);
            Email = Marshal.StringToHGlobalUni(email);
            Password = Marshal.StringToHGlobalUni(password);
            Next = null;
        }

        public void FreeData()
        {
            if (Names != IntPtr.Zero) Marshal.FreeHGlobal(Names);
            if (LastNames != IntPtr.Zero) Marshal.FreeHGlobal(LastNames);
            if (Email != IntPtr.Zero) Marshal.FreeHGlobal(Email);
            if (Password != IntPtr.Zero) Marshal.FreeHGlobal(Password);
        }

        public string? GetNames() => Marshal.PtrToStringUni(Names);
        public string? GetLastNames() => Marshal.PtrToStringUni(LastNames);
        public string? GetEmail() => Marshal.PtrToStringUni(Email);
        public string? GetPassword() => Marshal.PtrToStringUni(Password);

        public override string ToString() => $"Id: {Id}, Names: {GetNames()}, Last Names: {GetLastNames()}, Email: {GetEmail()}, Password: {GetPassword()}";
    }
}