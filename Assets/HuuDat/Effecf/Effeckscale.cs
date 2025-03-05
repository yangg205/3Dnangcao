using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[ExecuteInEditMode]
public class ParticleScaler : MonoBehaviour
{
    private ParticleSystem m_System;
    private ParticleSystem.Particle[] m_Particles;
    [SerializeField] private float m_Size = 1.0f; // Để có thể điều chỉnh trong Inspector
    [SerializeField] private float m_StartSpeed = 1.0f;

    private void LateUpdate()
    {
        InitializeIfNeeded();

        // Lấy số lượng hạt đang sống
        int numParticlesAlive = m_System.GetParticles(m_Particles);

        // Tính toán tỷ lệ hiện tại
        float currentScale = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3.0f;
        m_System.startSpeed = m_StartSpeed * currentScale;

        // Cập nhật kích thước của từng hạt
        for (int i = 0; i < numParticlesAlive; i++)
        {
            m_Particles[i].size = currentScale;
        }

        // Đặt lại các hạt đã cập nhật
        m_System.SetParticles(m_Particles, numParticlesAlive);
    }

    private void InitializeIfNeeded()
    {
        if (m_System == null)
        {
            m_System = GetComponent<ParticleSystem>();
        }

        if (m_Particles == null || m_Particles.Length < m_System.main.maxParticles)
        {
            m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];
        }
    }
}