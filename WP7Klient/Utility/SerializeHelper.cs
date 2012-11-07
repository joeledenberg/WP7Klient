using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Windows;

namespace WP7Klient.Utility
{
    public static class SerializeHelper
    {
        public static void SaveSetting<T>(string fileName, T dataToSave)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    using (var stream = store.CreateFile(fileName))
                    {
                        var serializer = new DataContractSerializer(typeof(T));
                        serializer.WriteObject(stream, dataToSave);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return;
                }
            }
        }
        public static T LoadSetting<T>(string fileName)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!store.FileExists(fileName))
                    return default(T);
                try
                {
                    using (var stream = store.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                    {
                        var serializer = new DataContractSerializer(typeof(T));
                        return (T)serializer.ReadObject(stream);
                    }
                }
                catch (Exception e)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(
                        () => MessageBox.Show(e.Message));
                    return default(T);
                }
            }
        }

    }
}
