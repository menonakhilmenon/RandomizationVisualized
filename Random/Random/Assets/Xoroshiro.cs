using Int = System.UInt64;
public class Xoroshiro
{
    static Int[] s;
    static RandomMersenne r;
    /// <summary>
    /// Sets initial values of state array
    /// </summary>
    public static void Initialize()
    {
        s = new Int[4];
        r = new RandomMersenne(0);
        s[0] = 0x01d353e5f3993bb0;
        s[1] = 0x7b9c0df6cb193b20;
        s[2] = 0xfdfcaa91110765b6;
        s[3] = 0x2d24cbe0ef44dcd2;
    }
    public static Int Range(int x,Int y)
    {
        Int result = rotl(s[1] * 5, 7) * 9;
        Int t = s[1] << 17;

        s[2] ^= s[0];
        s[3] ^= s[1];
        s[1] ^= s[2];
        s[0] ^= s[3];

        s[2] ^= t;

        s[3] = rotl(s[3], 45);
        return result%y;
    }

    public static int Range_(int x,int y)
    {
        return r.Range(x, y);
    }
    static Int rotl(Int x,int k)
    {
        return (x << k) | (x >> (64 - k));
    }
    
}
