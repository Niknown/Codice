namespace Gioco
{
    /// <summary>
    /// classe contenente le coordinate del giocatore e delle stanze.
    /// </summary>
    public class Coordinate
        {   
            public int X { get; set; }
            public int Y { get; set; }
            public Coordinate(int x, int y)
            {
                X = x;
                Y = y;
            }
            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + X.GetHashCode();
                    hash = hash * 23 + Y.GetHashCode();
                    return hash;
                }
            }

            public override bool Equals(object obj)
            {
                if (obj == null || !(obj is Coordinate))
                    return false;

                Coordinate other = (Coordinate)obj;
                return X == other.X && Y == other.Y;
            }
        }
}