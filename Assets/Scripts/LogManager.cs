using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

/* Tipos:
 * 
 * Lista(registrados periodicamente possuindo informações pontuais):
 * 
     * Movimento do Jogador = 0 (tipo = 0, tempo, x, y) 
     * Tiro do Jogador = 1 (tipo = 1, tempo, acertou 0 ou 1)
     * Dano do Jogador tomado = 2 (tipo = 2, tempo, quem (0 = zombunny, 1 = zombear, 2 = hellephant, 3 = player), quanto
     * Power up do Jogador = 3 (tipo = 3, tempo, qual)
     * Score obtido = 4 (tipo = 4, tempo, quanto)
     * Distancia com o outro Jogador = 5 (tipo = 5, tempo, modulo)
     * Dano realizado pelo jogador = 6 (tipo = 6, tempo, quem (0 = zombunny, 1 = zombear, 2 = hellephant, 3 = player, 4 = nada), quanto
     * 
     * Morte time 7 
     * Score Final 8
     * Game over time 9
 *
 * Por segundo:
 * 
     * Tiro por segundo = 0  (tipo = 0, tempo, quantia)
     * Acerto por segundo = 1 (tipo = 1, tempo, quantia)
     * Dano realizado por segundo (tipo = 2, tempo, quantia)
     * Dano tomado por segundo (tipo = 3, tempo, quantia)
     * Pontos por segundo (tipo = 4, tempo, quantia)
     * Inimigos a distancia (tipo = 5, tempo, quantia)
     * Inimigos atingindo (tipo = 6, tempo, quantia)
     * Inimigos Caçando (tipo = 7, tempo, quantia)
     * 
     * 
     * Score Final
     * Game over time
*/

public class LogManager : MonoBehaviour {
    static float timeOfGame;
    float timeToWrite = 5.0f;
    float secCounter;
    float listCounter;
    string fileName = "log_player_";
    public static bool gameOver;

    public static int[] playerShootPerSecond;
    public static int[] playerHittedPerSecond;
    public static int[] playerDamageMadePerSecond;
    public static int[] playerDamageTakenPerSecond;
    public static int[] playerScoreMadePerSecond;
    public static int[] enemiesAroundPerSecond;
    public static int[] enemiesClosePerSecond;
    public static int[] enemiesChasingPerSecond;

    public static List<String>[] playerMovement;
    public static List<String>[] playerShoot;
    public static List<String>[] playerDamageTaken;
    public static List<String>[] playerPowerPicked;
    public static List<String>[] playerScoreMade;
    public static List<String>[] playerDistance;
    public static List<String>[] playerDamageMade;

    void Awake() {
        timeOfGame = 0.0f;
        secCounter = 1.0f;
        listCounter = timeToWrite;

        playerShootPerSecond = new int[2];
        playerHittedPerSecond = new int[2];
        playerDamageMadePerSecond = new int[2];
        playerDamageTakenPerSecond = new int[2];
        playerScoreMadePerSecond = new int[2];
        enemiesAroundPerSecond = new int[2];
        enemiesClosePerSecond = new int[2];
        enemiesChasingPerSecond = new int[2];

        playerMovement = new List<String>[2];
        playerShoot = new List<String>[2];
        playerDamageTaken = new List<String>[2];
        playerPowerPicked = new List<String>[2];
        playerScoreMade = new List<String>[2];
        playerDistance = new List<String>[2];
        playerDamageMade = new List<String>[2];

        for (int i = 0; i < 2; i++)
        {
            playerMovement[i] = new List<String>();
            playerShoot[i] = new List<String>();
            playerDamageTaken[i] = new List<String>();
            playerPowerPicked[i] = new List<String>();
            playerScoreMade[i] = new List<String>();
            playerDistance[i] = new List<String>();
            playerDamageMade[i] = new List<String>();

            enemiesAroundPerSecond[i] = 0;
            enemiesClosePerSecond[i] = 0;
            enemiesChasingPerSecond[i] = 0;
        }

        FlushNormalVariables();
        FlushListVariables();
    }

    void FlushNormalVariables()
    {
        for (int i = 0; i < 2; i++)
        {
            playerShootPerSecond[i] = 0;
            playerHittedPerSecond[i] = 0;
            playerDamageMadePerSecond[i] = 0;
            playerDamageTakenPerSecond[i] = 0;
            playerScoreMadePerSecond[i] = 0;
        }
    }

    void FlushListVariables()
    {
        for (int i = 0; i < 2; i++)
        {
            if(playerMovement[i].Count > 0)
            {
                playerMovement[i].Clear();
            }
            if (playerShoot[i].Count > 0)
            {
                playerShoot[i].Clear();
            }
            if (playerDamageTaken[i].Count > 0)
            {
                playerDamageTaken[i].Clear();
            }
            if (playerPowerPicked[i].Count > 0)
            {
                playerPowerPicked[i].Clear();
            }
            if (playerScoreMade[i].Count > 0)
            {
                playerScoreMade[i].Clear();
            }
            if (playerDistance[i].Count > 0)
            {
                playerDistance[i].Clear();
            }
            if (playerDamageMade[i].Count > 0)
            {
                playerDamageMade[i].Clear();
            }
        }
    }

    public static void AddPlayerMovementLog(int player, Transform pos)
    {
        playerMovement[player-1].Add(LogManager.timeOfGame + " " + pos.position.x + " " + pos.position.z);
    }

    public static void AddPlayerShootLog(int player, int success)
    {
        playerShoot[player - 1].Add(LogManager.timeOfGame + " " +success);
    }

    public static void AddPlayerDamageMade(int player, int type, int quantity)
    {
        playerDamageMade[player - 1].Add(LogManager.timeOfGame + " " + type + " " + quantity);
    }

    public static void AddPlayerDamageLog(int player, int source, int quantity)
    {
        playerDamageTaken[player - 1].Add(LogManager.timeOfGame + " " + source + " " + quantity);
    }

    public static void AddPlayerPowerPicked(int player, int type)
    {
        playerPowerPicked[player - 1].Add(LogManager.timeOfGame + " " + type);
    }

    public static void AddPlayerScoreMade(int player, int quantity)
    {
        playerScoreMade[player - 1].Add(LogManager.timeOfGame + " " + quantity);
    }

    public static void AddPlayerDistanceLog(int player, float distance)
    {
        playerDistance[player - 1].Add(LogManager.timeOfGame + " " + distance);
    }

    void IterateVariableList(int type, List<String> list, StreamWriter sw)
    {
        for(int i = 0; i < list.Count; i++)
        {
            sw.WriteLine(type + " "+ list[i]);
        }
    }

    void WriteListInformations(StreamWriter sw, int player)
    {
        if(playerMovement[player].Count > 0)
        {
            IterateVariableList(0, playerMovement[player], sw);
        }
        if (playerShoot[player].Count > 0)
        {
            IterateVariableList(1, playerShoot[player], sw);
        }
        if (playerDamageTaken[player].Count > 0)
        {
            IterateVariableList(2, playerDamageTaken[player], sw);
        }
        if (playerPowerPicked[player].Count > 0)
        {
            IterateVariableList(3, playerPowerPicked[player], sw);
        }
        if (playerScoreMade[player].Count > 0)
        {
            IterateVariableList(4, playerScoreMade[player], sw);
        }
        if (playerDistance[player].Count > 0)
        {
            IterateVariableList(5, playerDistance[player], sw);
        }
        if (playerDamageMade[player].Count > 0)
        {
            IterateVariableList(6, playerDamageMade[player], sw);
        }
    }

    void WriteNormalInformations(StreamWriter sw, int player)
    {
        if(playerShootPerSecond[player] != 0)
            sw.WriteLine("0 " + timeOfGame + " " + playerShootPerSecond[player]);
        if (playerHittedPerSecond[player] != 0)
            sw.WriteLine("1 " + timeOfGame + " " + playerHittedPerSecond[player]);
        if (playerDamageMadePerSecond[player] != 0)
            sw.WriteLine("2 " + timeOfGame + " " + playerDamageMadePerSecond[player]);
        if (playerDamageTakenPerSecond[player] != 0)
            sw.WriteLine("3 " + timeOfGame + " " + playerDamageTakenPerSecond[player]);
        if (playerScoreMadePerSecond[player] != 0)
            sw.WriteLine("4 " + timeOfGame + " " + playerScoreMadePerSecond[player]);
        if (enemiesAroundPerSecond[player] != 0)
            sw.WriteLine("5 " + timeOfGame + " " + enemiesAroundPerSecond[player]);
        if (enemiesClosePerSecond[player] != 0)
            sw.WriteLine("6 " + timeOfGame + " " + enemiesClosePerSecond[player]);
        if (enemiesChasingPerSecond[player] != 0)
            sw.WriteLine("7 " + timeOfGame + " " + enemiesChasingPerSecond[player]);
    }

    void OnApplicationQuit()
    {
        WriteOnGameOver();
    }

    public void WriteOnGameOver()
    {
        for (int i = 0; i < 2; i++)
        {
            string path = @"Logs\" + fileName + (i + 1) + "_periodic.txt";
            if (File.Exists(path)) {
                using (StreamWriter sw = File.AppendText(path))
                {
                    WriteListInformations(sw, i);
                    if(i == 0)
                        sw.WriteLine("8 " + timeOfGame + ScoreManager.score1);
                    else if (i == 1)
                        sw.WriteLine("8 " + timeOfGame + ScoreManager.score2);

                    sw.WriteLine("9 " + timeOfGame);
                }
            }
            FlushListVariables();
        }
    }
    void Update () {
        timeOfGame += Time.deltaTime;
        secCounter -= Time.deltaTime;
        listCounter -= Time.deltaTime;
        if(secCounter <= 0 && !LogManager.gameOver)
        {
            for (int i = 0; i < 2; i++)
            {
                string path = @"Logs\" + fileName + (i+1) + "_persecond.txt";
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(DateTime.Now + "\n");
                        WriteNormalInformations(sw, i);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        WriteNormalInformations(sw, i);
                    }
                }
            }
            FlushNormalVariables();
            secCounter = 1.0f;
        }
        if(listCounter <= 0)
        {
            for (int i = 0; i < 2; i++)
            {
                string path = @"Logs\" + fileName + (i + 1) + "_periodic.txt";
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(DateTime.Now + "\n");
                        WriteListInformations(sw, i);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        WriteListInformations(sw, i);
                    }
                }
            }
            listCounter = timeToWrite;
            FlushListVariables();
        }
    }
}
