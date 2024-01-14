using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public Transform player;

    public int Gold =200;
    public GameObject ShopUI;
    [SerializeField] private Transform _shopContent;
    [SerializeField] private GameObject _itemPrefeb;
    [SerializeField] private Upgrade[] upgrades;
    [SerializeField] private GameObject[] _UpgradeUI;
    [SerializeField] private List<Upgrade> purchased = new List<Upgrade>();
    [SerializeField] private Text _goldText;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private CharacterStats _characterStats;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private SwordAttackController _sword;
    GameObject _item;
    private void Awake()
    {
        instance = this;
        _goldText.text = Gold.ToString();
    }
    private void Start()
    {
        foreach(Upgrade upgrade in upgrades)
        {
            _item = Instantiate(_itemPrefeb, _shopContent);

            upgrade.ItemRef = _item;
            foreach(Transform property in _item.transform)
            {
                if(property.name == "Image")
                property.GetComponent<Image>().sprite = upgrade.ImageObj;
                if (property.name == "Quantity")
                {
                    Transform quantity= property.GetChild(0).transform;
                    quantity.GetComponent<Text>().text = upgrade.Quantity.ToString();
                }
                if (property.name == "Price")
                {
                    Transform price = property.GetChild(0).transform;
                    price.GetComponent<Text>().text = upgrade.Cost.ToString();
                }
            }

            _item.GetComponent<Button>().onClick.AddListener(() => { AddToCart(upgrade); 
            });
        }

    }

    public void AddToCart(Upgrade upgrade)
    {
        if(Gold >= upgrade.Cost)
        {
            ChangeGold(-upgrade.Cost);
            upgrade.Quantity++;
            upgrade.ItemRef.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = upgrade.Quantity.ToString();
            if (!purchased.Contains(upgrade))
                purchased.Add(upgrade);
        }
    }

    public void OnBuy()
    {
        ToggleShop(false);
        ApplyUpgrade();
        purchased.Clear();
        ResetQuantities();
    }
    private void ApplyUpgrade()
    {
        if (purchased.Count == 1)
        {
            Upgrade singleUpgrade = purchased[0];
            if (singleUpgrade.Quantity > 1)
            {
                Debug.Log(singleUpgrade.Name + " x" + singleUpgrade.Quantity);
                if (purchased[0].Name == "Health Crystal")
                {
                    Debug.Log("Lion's Heart");
                    SetUpgradeUI("Lion's Heart");
                    _characterStats.SetMaxHealth(300);
                }
            }
            else
            {
                SetUpgradeUI(singleUpgrade.Name);
                SetNewPlayerStats(singleUpgrade.Name);
            }
        }
        else if (purchased.Count >= 2)
        {
            if (purchased.Any(upgrade => upgrade.Name == "Health Crystal") &&
                purchased.Any(upgrade => upgrade.Name == "Leather Gloves"))
            {
                SetUpgradeUI("Worrior's Armor");
                _characterStats.SetMaxHealth(150);
                _bullet.SetBulletDamage(30);
                _sword.SetDamageOffset(30);
            }
            else
            {
                List<Upgrade> purchasedList = new List<Upgrade>(purchased);

                StartCoroutine(ProcessUpgradesSequentially(purchasedList));
            }
        }
    }
    private IEnumerator ProcessUpgradesSequentially(List<Upgrade> upgrades)
    {
        foreach (var upgrade in upgrades)
        {
            Debug.Log(upgrade.Name + " x" + upgrade.Quantity);
            SetUpgradeUI(upgrade.Name);
            SetNewPlayerStats(upgrade.Name);

            // Wait for a short duration before moving to the next upgrade
            yield return new WaitForSeconds(0.5f);

            // Hide the UI after showing
            DeactivateUpgradeUI(upgrade.Name);
        }
    }


    void SetNewPlayerStats(string name)
    {
        if (name == "Health Crystal")
        {
            _characterStats.SetMaxHealth(50);
        }
        if (name == "Leather Gloves")
        {
            _bullet.SetBulletDamage(10);
            _sword.SetDamageOffset(10);
        }
        if (name == "Magic Boots")
        {
            _playerController.SetSpeed(5f);
        }
    }
    public void ResetQuantities()
    {
        foreach (Upgrade upgrade in upgrades)
        {
            upgrade.Quantity = 0;
            // Update the UI to display the reset quantity
            upgrade.ItemRef.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = upgrade.Quantity.ToString();
        }
    }
    public void SetUpgradeUI(string lookName)
    {
        foreach (GameObject upgradeUI in _UpgradeUI)
        {
            if (upgradeUI.name == lookName)
            {
                upgradeUI.SetActive(true);
                StartCoroutine(DeactivateUpgradeUI(lookName));
            }
        }
    }

    private IEnumerator DeactivateUpgradeUI(string activeName)
    {
        yield return new WaitForSeconds(0.8f);
        foreach (GameObject upgradeUI in _UpgradeUI)
        {
            if (upgradeUI.name == activeName)
            {
                upgradeUI.SetActive(false);
            }
        }
    }
    public void ToggleShop(bool value)
    {
        ShopUI.SetActive(value);
    }
    public void ChangeGold(int value)
    {
        Gold += value;
        _goldText.text = Gold.ToString();
    }
    public int GetGold()
    {
        return Gold;
    }
    public void ResetGold(int value)
    {
        Gold = value;
        _goldText.text = Gold.ToString();
    }
}
