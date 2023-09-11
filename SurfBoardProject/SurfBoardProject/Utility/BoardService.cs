using SurfBoardProject.Models.Enum;
using SurfBoardProject.Models;

namespace SurfBoardProject.Utility
{
    public class BoardService
    {
        public void ImageAndBoardSelector(BoardModel board)
        {
            if (board.BoardType == BoardType.Fish)
            {
                board.BoardDescription = "Fish";
                board.ImgUrl = "\\Images\\FishBoard.jpg";

            }
            else if (board.BoardType == BoardType.Funboard)
            {
                board.BoardDescription = "Funboard";
                board.ImgUrl = "\\Images\\Funboard.jpg";

            }
            else if (board.BoardType == BoardType.Shortboard)
            {
                board.BoardDescription = "ShortBoard";
                board.ImgUrl = "\\Images\\Shortboard.jpg";

            }
            else if (board.BoardType == BoardType.Longboard)
            {
                board.BoardDescription = "Longboard";
                board.ImgUrl = "\\Images\\Longboard.jpg";
            }
            else if (board.BoardType == BoardType.SUP)
            {

                board.ImgUrl = "\\Images\\SUP.jpeg";
                board.BoardDescription = "SUP";
            }
        }

    }
}
