using UnityEngine;
using UnityEngine.InputSystem;

public class Doors : MonoBehaviour
{
    [Header("Réglages")]
    public Vector3 decalageOuvert = new Vector3(0, 3f, 0);
    public float vitesse = 2f;

    [Header("Etat (cochable pour tester)")]
    public bool ouverte;

    [Header("Salle suivante")]
    public GameObject[] zonesSalleSuivante;

    Vector3 posFermee;
    Vector3 posOuverte;

    void Start()
    {
        posFermee = transform.localPosition;
        posOuverte = posFermee + decalageOuvert;
        ActiverZones(ouverte);
    }

    void Update()
    {
    
        if (Keyboard.current != null && Keyboard.current.oKey.wasPressedThisFrame)
            Basculer();

        Vector3 cible = ouverte ? posOuverte : posFermee;
        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition, cible, vitesse * Time.deltaTime);
    }

    void ActiverZones(bool etat)
    {
        foreach (var z in zonesSalleSuivante)
            if (z != null) z.SetActive(etat);
    }

    [ContextMenu("Ouvrir")]
    public void Ouvrir()
    {
        ouverte = true;
        ActiverZones(true);
    }

    [ContextMenu("Fermer")]
    public void Fermer()
    {
        ouverte = false;
        ActiverZones(false);
    }

    [ContextMenu("Basculer")]
    public void Basculer()
    {
        if (ouverte) Fermer();
        else Ouvrir();
    }
}