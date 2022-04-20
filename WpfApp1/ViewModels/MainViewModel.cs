using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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
    public class MainViewModel : INotifyPropertyChanged
    {
        public bool Parallel { get => _filters.Parallel; set => _filters.Parallel = value;}
        private string _originalFilename;
        private Filters _filters;
        private BitmapImage _bitmapImage;
        public BitmapImage BitmapImage
        {
            get => _bitmapImage;
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
                        OpenFileDialog openFileDialog = new OpenFileDialog
                        {
                            Filter = " (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg"
                        };

                        if (openFileDialog.ShowDialog() == true)
                        {
                            BitmapImage = new BitmapImage(new Uri(openFileDialog.FileName));
                            _originalFilename = openFileDialog.FileName;
                            
                        }

                    },
                    () => true                    
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
                    () => true
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
                        _filters.Pixels = Array2DBMIConverter.BitmapImageToArray2D(BitmapImage);

                        _filters.FilterColor();

                        BitmapImage = Array2DBMIConverter.ConvertWriteableBitmapToBitmapImage(Array2DBMIConverter.Array2DToWriteableBitmap(_filters.Pixels, BitmapImage));



                    },
                    () =>
                    {
                        if(BitmapImage == null)
                            return false;
                        else
                            return true;
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
                        _filters.Pixels = Array2DBMIConverter.BitmapImageToArray2D(BitmapImage);

                        _filters.Reduction();

                        BitmapImage = Array2DBMIConverter.ConvertWriteableBitmapToBitmapImage(Array2DBMIConverter.Array2DToWriteableBitmap(_filters.Pixels, BitmapImage));



                    },
                    () =>
                    {
                        if(BitmapImage == null)
                            return false;
                        else
                            return true;
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
                        _filters.Pixels = Array2DBMIConverter.BitmapImageToArray2D(BitmapImage);

                        _filters.Flip();

                        BitmapImage = Array2DBMIConverter.ConvertWriteableBitmapToBitmapImage(Array2DBMIConverter.Array2DToWriteableBitmap(_filters.Pixels, BitmapImage));



                    },
                    () =>
                    {
                        if (BitmapImage == null)
                            return false;
                        else
                            return true;
                    }
                    ));
            }
        }
        private ICommand _resetCommand;
        public ICommand ResetCommand
        {
            get
            {
                return (_resetCommand = new CommandHandler(
              () =>
              {
                  BitmapImage = new BitmapImage(new Uri(_originalFilename));
              },
              () =>
              {
                  if (BitmapImage == null)
                      return false;
                  else
                      return true;
              }));
            }
        }
        private ICommand _blurCommand;
        public ICommand BlurCommand { get
            {
                return (_blurCommand = new CommandHandler(
                    () =>
                    {
                        _filters.Pixels = Array2DBMIConverter.BitmapImageToArray2D(BitmapImage);
                        using (MemoryStream stream = new MemoryStream())
                        {
                            BitmapEncoder encoder = new BmpBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(BitmapImage));
                            encoder.Save(stream);
                            Bitmap bitmap = new Bitmap(stream);
                            _filters.Blur(bitmap);
                        }

                        BitmapImage = Array2DBMIConverter.ConvertWriteableBitmapToBitmapImage(Array2DBMIConverter.Array2DToWriteableBitmap(_filters.Pixels, BitmapImage));
                    },
                    () =>
                    {
                        if (BitmapImage == null)
                            return false;
                        else
                            return true;
                    }));
            } 
        }
        private ICommand _redCommand;
        public ICommand RedCommand
        {
            get
            {
                return _redCommand = new CommandHandler(
                () =>
                {
                    _filters.Pixels = Array2DBMIConverter.BitmapImageToArray2D(BitmapImage);
                    _filters.Colors(0);
                    BitmapImage = Array2DBMIConverter.ConvertWriteableBitmapToBitmapImage(Array2DBMIConverter.Array2DToWriteableBitmap(_filters.Pixels, BitmapImage));
                },
                () =>
                {
                    if (BitmapImage == null)
                        return false;
                    else
                        return true;
                });
            }
        }
        private ICommand _greenCommand;
        public ICommand GreenCommand
        {
            get
            {
                return _greenCommand = new CommandHandler(
                () =>
                {
                    _filters.Pixels = Array2DBMIConverter.BitmapImageToArray2D(BitmapImage);
                    _filters.Colors(1);
                    BitmapImage = Array2DBMIConverter.ConvertWriteableBitmapToBitmapImage(Array2DBMIConverter.Array2DToWriteableBitmap(_filters.Pixels, BitmapImage));
                },
                () =>
                {
                    if (BitmapImage == null)
                        return false;
                    else
                        return true;
                });
            }
        }
        private ICommand _blueCommand;
        public ICommand BlueCommand
        {
            get
            {
                return _blueCommand = new CommandHandler(
                () =>
                {
                    _filters.Pixels = Array2DBMIConverter.BitmapImageToArray2D(BitmapImage);
                    _filters.Colors(2);
                    BitmapImage = Array2DBMIConverter.ConvertWriteableBitmapToBitmapImage(Array2DBMIConverter.Array2DToWriteableBitmap(_filters.Pixels, BitmapImage));
                },
                () =>
                {
                    if (BitmapImage == null)
                        return false;
                    else
                        return true;
                });
            }
        }
        public MainViewModel()
        {
            _filters = new Filters();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
