using UnityEngine;
using UnityEngine.Events;

public class PuzzleLeviers : MonoBehaviour
{
    [Header("Leviers")]
    [SerializeField] private int nombreLeviers = 4;
    [Tooltip("Numéros des leviers qui doivent être activés (0 = premier)")]
    [SerializeField] private int[] leviersRequis;
    [Tooltip("Si coché, les autres leviers doivent être désactivés")]
    [SerializeField] private bool exigerLesAutresEteints = true;

    [Header("Événements")]
    [SerializeField] private UnityEvent onResolu;
    [SerializeField] private UnityEvent onAnnule;

    private bool[] etats;
    private bool resolu;

    private void Awake()
    {
        etats = new bool[nombreLeviers];
    }

    public void Activer(int index) => SetEtat(index, true);
    public void Desactiver(int index) => SetEtat(index, false);
    public void Basculer(int index) => SetEtat(index, !etats[index]);

    private void SetEtat(int index, bool valeur)
    {
        if (index < 0 || index >= etats.Length) return;

        etats[index] = valeur;
        Verifier();
    }

    private void Verifier()
    {
        bool ok = true;

        for (int i = 0; i < etats.Length; i++)
        {
            bool requis = System.Array.IndexOf(leviersRequis, i) >= 0;

            if (requis && !etats[i]) ok = false;
            if (!requis && exigerLesAutresEteints && etats[i]) ok = false;
        }

        if (ok && !resolu)
        {
            resolu = true;
            onResolu?.Invoke();
        }
        else if (!ok && resolu)
        {
            resolu = false;
            onAnnule?.Invoke();
        }
    }
}