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
                var item = App.ViewModel.getTopIncompletedItem();
                StandardTileData NewTileData = new StandardTileData()
                {
                    Title = "ToDo",
                    BackgroundImage = new Uri("ApplicationIcon.png", UriKind.Relative),
                    Count = App.ViewModel.getIncompletedItemCount(),
                    BackTitle = (item == null) ? "" : "Urgent",
                    BackBackgroundImage = new Uri("SplashScreenImage.jgp", UriKind.Relative),
                    BackContent = (item == null) ? "Good Job !" : item.Title

                };
                TileToFind.Update(NewTileData);
            }
        }
    }
}
