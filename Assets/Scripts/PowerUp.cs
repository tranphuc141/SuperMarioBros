using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type
    {
        coin,
        ExtraLife,
        MagicMushroom,
        Starpower,
    }  
    public Type type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect(other.gameObject);
        }
    }

    private void Collect(GameObject player)
    {
        switch (type)
        {
            case Type.coin:
            GameManager.Instance.AddCoin();

                break;

            case Type.ExtraLife:
            GameManager.Instance.AddLife();

                break;

            case Type.MagicMushroom:
            player.GetComponent<Player>().Grow();

                break;

            case Type.Starpower:
            player.GetComponent<Player>().StarPower();

                break;
        }

        Destroy(gameObject);
    }
}
