namespace SpaceInvaders.Model
{
    public enum PhysicsLayer
    {
        World        = 0b0000_0001,
        Player       = 0b0000_0010,
        Enemy        = 0b0000_0100,
        PlayerHitbox = 0b0000_1000,
        EnemyHitbox  = 0b0001_0000
    }
}