using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public float health = 50f;
    private PlayerStats player_stats;

    public void TakeDamage(float amount)
    {
        health -= amount;

        player_stats = GetComponent<PlayerStats>();
        player_stats.Display_HealthStats(health);

        if (health <= 0f)
        {
            if (this.gameObject.tag == Tags.PLAYER_TAG)
            {
                DiePlayer();
            }
            else
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void DiePlayer()
    {
        SceneManager.LoadScene("GameOver");
    }
}
