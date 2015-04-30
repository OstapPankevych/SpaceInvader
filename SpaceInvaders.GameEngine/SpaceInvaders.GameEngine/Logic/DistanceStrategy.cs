using SpaceInvaders.GameEngine.Objects;

namespace SpaceInvaders.GameEngine.Logic
{
   public class DistanceStrategy:IDistanceStrategy
    {
        public double GetDistance(GameObject obj1, GameObject obj2)
        {
            if (((obj1.PosX - 1) <= obj2.PosX) && ((obj1.PosX + 3) >= obj2.PosX))
            {
                return obj2.PosY - obj1.PosY;
            }

            return 1.0;
        }

   }
}
