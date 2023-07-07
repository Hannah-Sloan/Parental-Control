using System;

public class PlayerDeath : Singleton<PlayerDeath>
{
    public Action PlayerDeathEvent;
}
