using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public Arrow m_ArrowPrefab;
    public Transform m_ArrowSpawn;
    public float m_ReloadTime;
    public Bowstring m_Bowstring;
    public float m_DrawSpeed;
    public float m_DrawDistance;

    private bool m_IsReloading;
    private bool m_IsDrawn = false;
    private Arrow m_CurrentArrow;

    // Start is called before the first frame update
    void Start()
    {
        //m_ArrowSpawn = GetComponentInChildren<Transform>();
        Reload();
        //ShootArrow();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsReady()) return;
        Pull();
        ShootArrow();
    }


    [ContextMenu("Shoot Arrow")]
    private void ShootArrow()
    {
        
        if (!m_IsDrawn) return;

        /*
        var direction = m_CurrentArrow.transform.forward;
        var position = m_CurrentArrow.transform.position;

        var endPosition = m_ArrowSpawn.position;
        */
        m_CurrentArrow.Fire();
        m_CurrentArrow = null;

        /*
        while (position != endPosition)
        {
            position += direction * m_DrawSpeed * 4 * Time.deltaTime;
        }
        */


        m_Bowstring.DrawBowstring(m_ArrowSpawn);

        m_IsDrawn = false;
        Reload();
    }

    [ContextMenu("Pull Arrow")]
    private void Pull()
    {
        Debug.Log($"Pull");
        if (m_IsDrawn) return;

        var direction = m_CurrentArrow.transform.forward;
        var position = m_CurrentArrow.transform.position;

        var endPosition = position - direction * m_DrawDistance;

        m_CurrentArrow.m_Body.MovePosition(endPosition);
        m_Bowstring.DrawBowstring(m_CurrentArrow.transform);

        /*
        while (position != endPosition)
        {
            position -= direction * m_DrawSpeed * Time.deltaTime;

            m_CurrentArrow.m_Body.MovePosition(position);
            m_Bowstring.DrawBowstring(m_CurrentArrow.transform);
        }
        */

        m_IsDrawn = true;
    }


    [ContextMenu("Reload")]
    public void Reload()
    {
        if (m_IsReloading || m_CurrentArrow != null) return;

        m_IsReloading = true;
        StartCoroutine(ReloadAfterTime());
    }

    private IEnumerator ReloadAfterTime()
    {
        yield return new WaitForSeconds(m_ReloadTime);

        m_CurrentArrow = Instantiate(m_ArrowPrefab, m_ArrowSpawn.position, m_ArrowSpawn.rotation);
        m_Bowstring.DrawBowstring(m_ArrowSpawn);

        m_CurrentArrow.m_Body.isKinematic = true;

        //Debug.Log($"{m_CurrentArrow.transform.position} | {m_ArrowSpawn.position}");
        m_IsReloading = false;
    }

    public bool IsReady() => (!m_IsReloading && m_CurrentArrow != null);
}