using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfApp1.Model;

namespace WpfApp1.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private ColorHelper _colorHelper;
        private BitmapImage _bitmapImage;
        public BitmapImage BitmapImage
        {
            get
            {
                return _bitmapImage;
            }
            set
            {
                _bitmapImage = value;
                NotifyPropertyChanged();
            }
        }

        private ICommand _filePickedCommand;
        public ICommand FilePickedCommand
        {
            get
            {
                return (_filePickedCommand = new CommandHandler(
                    () =>
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = " (*.png;*.jpeg)|*.png;*.jpeg";

                        if (openFileDialog.ShowDialog() == true)
                        {
                            BitmapImage = new BitmapImage(new Uri(openFileDialog.FileName));
                            
                        }

                    },
                    () =>
                    {
                        return true;
                    }
                    ));
            }
        }
        public ICommand FileUnloadCommand
        {
            get
            {
                return (_filePickedCommand = new CommandHandler(
                    () =>
                    {
                        BitmapImage = null;

                    },
                    () =>
                    {
                        return true;
                    }
                    ));
            }
        }

        private ICommand _blackFadeCommand;
        public ICommand BlackFadeCommand
        {
            get
            {
                return (_blackFadeCommand = new CommandHandler(
                    () =>
                    {
                        _colorHelper.Pixels = Array2DBMIConverter.BitmapImageToArray2D(BitmapImage);

                        _colorHelper.FilterColor();

                        BitmapImage = Array2DBMIConverter.ConvertWriteableBitmapToBitmapImage(Array2DBMIConverter.Array2DToWriteableBitmap(_colorHelper.Pixels, BitmapImage));



                    },
                    () =>
                    {
                        return BitmapImage == null? false: true;
                    }
                    ));
            }
        }
        private ICommand _ReductionCommand;
        public ICommand ReductionCommand
        {
            get
            {
                return (_ReductionCommand = new CommandHandler(
                    () =>
                    {
                        _colorHelper.Pixels = Array2DBMIConverter.BitmapImageToArray2D(BitmapImage);

                        _colorHelper.Reduction();

                        BitmapImage = Array2DBMIConverter.ConvertWriteableBitmapToBitmapImage(Array2DBMIConverter.Array2DToWriteableBitmap(_colorHelper.Pixels, BitmapImage));



                    },
                    () =>
                    {
                        return BitmapImage == null ? false : true;
                    }
                    ));
            }
        }

        private ICommand _flipCommand;
        public ICommand FlipCommand
        {
            get
            {
                return (_flipCommand = new CommandHandler(
                    () =>
                    {
                        _colorHelper.Pixels = Array2DBMIConverter.BitmapImageToArray2D(BitmapImage);

                        _colorHelper.Flip();

                        BitmapImage = Array2DBMIConverter.ConvertWriteableBitmapToBitmapImage(Array2DBMIConverter.Array2DToWriteableBitmap(_colorHelper.Pixels, BitmapImage));



                    },
                    () =>
                    {
                        return BitmapImage == null ? false : true;
                    }
                    ));
            }
        }

        public BaseViewModel()
        {
            _colorHelper = new ColorHelper();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
