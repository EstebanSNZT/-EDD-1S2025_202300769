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

        public string ToGraph()
        {
            return $"[label = \"{{<data> ID: {Id} \\n Nombre: {GetNames() + GetLastNames()} \\n Correo: {GetEmail()} \\n Contraseña: {GetPassword()}}}\"];";
        }

        public void UpdateNode(string newNames, string newLastNames, string newEmail)
        {
            SetNames(newNames);
            SetLastNames(newLastNames);
            SetEmail(newEmail);
        }

        public void SetNames(string newNames)
        {
            if (!string.IsNullOrEmpty(newNames) && newNames != GetNames())
            {
                if (Names != IntPtr.Zero) Marshal.FreeHGlobal(Names);
                Names = Marshal.StringToHGlobalUni(newNames);
            }
        }

        public void SetLastNames(string newLastNames)
        {
            if (!string.IsNullOrEmpty(newLastNames) && newLastNames!= GetLastNames())
            {
                if (LastNames!= IntPtr.Zero) Marshal.FreeHGlobal(LastNames);
                LastNames = Marshal.StringToHGlobalUni(newLastNames);
            }
        }

        public void SetEmail(string newEmail)
        {
            if (!string.IsNullOrEmpty(newEmail) && newEmail!= GetEmail())
            {
                if (Email!= IntPtr.Zero) Marshal.FreeHGlobal(Email);
                Email = Marshal.StringToHGlobalUni(newEmail);
            }
        }

        public void SetPassword(string newPassword)
        {
            if (!string.IsNullOrEmpty(newPassword) && newPassword!= GetPassword())
            {
                if (Password!= IntPtr.Zero) Marshal.FreeHGlobal(Password);
                Password = Marshal.StringToHGlobalUni(newPassword);
            }
        }

        public string? GetNames() => Marshal.PtrToStringUni(Names);
        public string? GetLastNames() => Marshal.PtrToStringUni(LastNames);
        public string? GetEmail() => Marshal.PtrToStringUni(Email);
        public string? GetPassword() => Marshal.PtrToStringUni(Password);

        public override string ToString() => $"Id: {Id}, Nombres: {GetNames()}, Apellidos: {GetLastNames()}, Correo: {GetEmail()}, Contraseña: {GetPassword()}";
    }
}