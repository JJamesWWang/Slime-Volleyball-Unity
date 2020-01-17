using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    // Game state related
    public const string GAME_STARTED = "GAME_STARTED";      //<>
    public const string POINT_STARTED = "POINT_STARTED";    //<Side>
    public const string POINT_ENDED = "POINT_ENDED";        //<Side>
    public const string GAME_ENDED = "GAME_ENDED";          //<Side>
    public const string GAME_PAUSED = "GAME_PAUSED";        //<>
    public const string GAME_UNPAUSED = "GAME_UNPAUSED";    //<>
    public const string GAME_RESET = "GAME_RESET";          //<>

    // Volleyball related
    public const string PLAYER_CONTACT = "PLAYER_CONTACT";  //<Volleyball, PlayerController>
    public const string WALL_CONTACT = "WALL_CONTACT";      //<Volleyball, Tilemap>
    public const string SCORE_AREA_CONTACT = "SCORE_AREA_CONTACT";      //<Volleyball, ScoreArea>
    public const string SPIKE_HIT = "SPIKE_HIT";            //<Volleyball, PlayerController>

    // UI related

    public const string SCORE_UPDATED = "SCORE_UPDATED";    //<int, int>

}
