using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LSBEncoding.Views
{
    public class PropertyNotifierClass : INotifyPropertyChanged
    {
        /// <summary>
        /// Method assigning new value to a property and firing property changed event
        /// </summary>
        /// <typeparam name="T">Type of value and property</typeparam>
        /// <param name="privateProperty">Property to change</param>
        /// <param name="newValue">New value</param>
        /// <param name="propertyName">Name of property that has been changed</param>
        protected virtual void SetProperty<T>(ref T privateProperty, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (Equals(privateProperty, newValue))
            {
                return;
            }
            privateProperty = newValue;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Changed event handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
