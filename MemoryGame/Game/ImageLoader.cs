using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MemoryGame
{
    public interface IAssetProvider
    {
        FrameworkElement GetAssetForId( int id );
    }

    class ImageLoader: IAssetProvider
    {
        public static readonly ImageLoader Instance = new ImageLoader();
        private static readonly Dictionary<int, BitmapImage> cache = new Dictionary<int, BitmapImage>();

        private readonly string[] files;

        public ImageLoader()
        {
            files = Directory.GetFiles( "Images" );
        }

        public FrameworkElement GetAssetForId( int id )
        {
            // Cached image
            if ( cache.ContainsKey( id ) )
                return new Image { Source = cache[id] };

            // Uncached image
            if ( id < files.Length )
            {
                var bitmap = LoadImageFromDisk( files[id] );
                cache[id] = bitmap;
                return new Image { Source = cache[id] };
            }

            // Stub
            return new TextBlock
            {
                Text = id.ToString(),
                FontSize = 36,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        private static BitmapImage LoadImageFromDisk( string fileName )
        {
            var path = Path.Combine( Environment.CurrentDirectory, fileName );
            return new BitmapImage( new Uri( path ) );
        }
    }
}
