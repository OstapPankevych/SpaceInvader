using SpaceInvaders.GameEngine.Objects;

namespace SpaceInvaders.GameEngine.Logic
{
    public interface IDistanceStrategy
    {
        double GetDistance(GameObject obj1, GameObject obj2); 
    }
}
