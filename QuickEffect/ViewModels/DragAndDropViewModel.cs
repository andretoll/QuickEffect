using NotebookWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuickEffect.ViewModel
{
    public class DragAndDropViewModel : BaseViewModel
    {
        #region Private members

        private string fileName;


        #endregion

        #region Public members

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

        #endregion

        #region Constructor

        #endregion

        #region Commands


        #endregion

        #region Methods

        public string[] GetFileNamesFromDrop(object e)
        {
            var files = (string[])((DragEventArgs)e).Data.GetData(DataFormats.FileDrop);

            return files;
        }

        #endregion
    }
}
