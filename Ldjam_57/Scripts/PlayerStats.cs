using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    int hp = 4;

    public int metalCount = 0;
    public int bioMaterialCount = 0;
    public int crystalCount = 0;

    public int processedAlloy = 0;
    public int bioGel = 0;
    public int powerCells = 0;

    public void Damage()
    {
        hp--;
    }

    public int GetHp()
    {
        return hp;
    }

    void Update()
    {
        if (hp <= 0)
        {
            SceneManager.LoadScene("DeathScene");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}