using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuickEffect.Models
{
    public class ImageItem : INotifyPropertyChanged
    {
        #region Private members

        private string fileName;
        private bool isSelected;

        #endregion

        #region Properties

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;

                NotifyPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;

                NotifyPropertyChanged();
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
