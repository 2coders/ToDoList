using System;
using Microsoft.Phone.Shell;
using System.Linq;

namespace ToDo.ViewModel
{
    public class TileModel
    {

        public static void updateTile()
        { 
            ShellTile TileToFind = ShellTile.ActiveTiles.First();
            if (TileToFind != null)
            {
                StandardTileData NewTileData = new StandardTileData()
                {
                    Title = "ToDo",
                    BackgroundImage = new Uri("ApplicationIcon.png", UriKind.Relative),
                    Count = App.ViewModel.getIncompletedItemCount(),
                    BackTitle = "backtitle",
                    BackBackgroundImage = new Uri("SplashScreenImage.jgp", UriKind.Relative),
                    BackContent = "backcontent"

                };
                TileToFind.Update(NewTileData);
            }
        }
    }
}
