using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    // Game state related
    public const string GAME_PAUSED = "GAME_PAUSED";
    public const string GAME_UNPAUSED = "GAME_UNPAUSED";
    public const string POINT_STARTED = "POINT_STARTED";
    public const string POINT_ENDED = "POINT_ENDED";
    public const string LEFT_TEAM_SCORED = "LEFT_TEAM_SCORED";
    public const string RIGHT_TEAM_SCORED = "RIGHT_TEAM_SCORED";
    public const string GAME_ENDED = "GAME_ENDED";


    // Volleyball related
    public const string PLAYER_CONTACT = "PLAYER_CONTACT";
    public const string WALL_CONTACT = "WALL_CONTACT";
    public const string SCORE_AREA_CONTACT = "SCORE_AREA_CONTACT";
    public const string SPIKE_HIT = "SPIKE_HIT";

}
