using NotebookWPF.Commands;
using NotebookWPF.ViewModel;
using System;
using System.Collections.Generic;
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

        private string fileName = "Drag & Drop files here.";

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

        //private ICommand dropFileCommand;
        //public ICommand DragFileCommand
        //{
        //    get
        //    {
        //        if (dropFileCommand == null)
        //            dropFileCommand = new RelayCommand(async p => { await DragFile(); }, p => true);
        //        return dropFileCommand;
        //    }
        //    set
        //    {
        //        dropFileCommand = value;
        //    }
        //}


        #endregion

        #region Methods

        public string DragFile(object e)
        {
            var files = (string[])((DragEventArgs)e).Data.GetData(DataFormats.FileDrop);
            return files[0];
        }

        #endregion
    }
}
