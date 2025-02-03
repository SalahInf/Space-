using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public Rock currentRock;
    public Animator animator;
    public Rigidbody _rb;
    public float speed;
    public float flySpeed;
    Camera camDir => Camera.main;
    [SerializeField] Joystick m_joystick => Root.Joystick;
    [SerializeField] float m_checkDistance;
    [SerializeField] bool isGrounded;
    [SerializeField] bool m_isFlying;
    [SerializeField] bool m_isJumping;

    [SerializeField] ParticleSystem m_hitRockFx;
    [SerializeField] LayerMask m_WhatIsGround;
    [SerializeField] Transform m_hitRockFxPos;
    [SerializeField] GameObject m_Axe;
    [SerializeField] GameObject m_jetPack;
    [SerializeField] GameObject m_wings;
    [SerializeField] float force;
    [SerializeField] ParticleSystem upgradeFX;
    //int resourceCount;
    [ReadOnly] public int axeDamage;
    [ReadOnly] public float axeSpeed;


    RaycastHit hit;

    #region Tube
    public Lines lines;
    public Transform m_startLine;
    #endregion

    #region reactorFill
    [SerializeField] Transform startdimePos;
    [SerializeField] Dimands dime;
    [SerializeField] Dimands Oil;
    [SerializeField] Dimands Electricity;
    bool canputInReactor;
    #endregion

    #region UiPlayer
    [SerializeField] TMP_Text m_UiGoldCounter;
    [SerializeField] TMP_Text m_UijemsCounter;
    [SerializeField] TMP_Text m_UiOilCounter;
    [SerializeField] TMP_Text m_UiElectricityCounter;
    [SerializeField] TMP_Text m_maxText;
    #endregion

    [SerializeField] SkinnedMeshRenderer m_rend;
    [SerializeField] Material m_targetHelmetMaterial;
    Material m_currentHelmetMaterial;

    float currentSpeed;
    private UpgradeArea upgradeArea;
    Vector3 m_startJetScale;
    SellPoint m_sellpoint;
    StuckMony stackMony;
    Coroutine fillReactor;
    GameObject currentTxtject;
    ElectricityGenerator generator;
    public bool isColecting;
    bool isfaling;

    private void Start()
    {
        m_startJetScale = m_jetPack.transform.localScale;
        currentSpeed = speed;
        Time.timeScale = 1.2f;
    }
    private void Update()
    {
        CheckGrounded();
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.05f, Vector3.down, out hit, m_checkDistance, m_WhatIsGround);
        isfaling = transform.position.y < -1;
        if (isfaling && !m_isFlying)
        {
            m_isFlying = true;
            isfaling = false;
            StartCoroutine(Fly(0.1f));

        }
        if (m_isFlying)
        {
            lines.FetchPosition(startdimePos.position);
        }
    }
    void MovePlayer()
    {
        if (m_isFlying)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, 0.4f, 0.2f), transform.position.z);
            if (_rb.constraints != RigidbodyConstraints.FreezePositionY)
                _rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }
        if (m_isJumping || !Root.GameManager.isPlaying)
            return;

        Vector3 right = m_joystick.Direction.x * camDir.transform.right;
        Vector3 forward = m_joystick.Direction.y * camDir.transform.forward;
        Vector3 dir = (right + forward);
        dir.y = 0;
        if (m_isFlying)
            _rb.velocity = Vector3.Slerp(_rb.velocity, dir.normalized * currentSpeed * m_joystick.Direction.magnitude, 0.25f);
        else
            _rb.MovePosition(_rb.position + dir.normalized * currentSpeed * m_joystick.Direction.magnitude * Time.deltaTime);

        transform.forward = Vector3.Slerp(transform.forward, dir.normalized, 0.2f);
        animator.SetFloat("Blend", Mathf.Lerp(animator.GetFloat("Blend"), m_joystick.Direction.magnitude, 0.25f));


    }

    IEnumerator FillReactor(float time, int count, Reactor r)
    {
        float t0 = 0;
        while (canputInReactor)
        {
            t0 += Time.deltaTime;

            if (t0 > time)
                break;

            yield return null;
        }

        if (canputInReactor)
        {
            r.isFilling = true;
            r.AddResources(count, time * count);
            Vector3 reactorStartScale = r.transform.localScale;
            float v = (time / count);
            float t = time;
            Root.GameManager.CurrentPlayerOil -= count;
            if (Root.GameManager.CurrentStorage > 0)
                Root.GameManager.CurrentStorage -= count;

            for (int i = 0; i < count; i++)
            {
                Dimands d = Instantiate(Oil, startdimePos.position, Quaternion.identity);
                d.transform.DOScale(Vector3.one * 2f, t).SetEase(Ease.InElastic);
                d.transform.DOJump(r.target.position, 1f, 1, t).SetEase(Ease.InFlash).OnComplete(() =>
                {
                    r.transform.DOScale(reactorStartScale * 0.97f, 0.1f).OnComplete(() => r.transform.DOScale(reactorStartScale, 0.1f));
                    r.UpdateValue();
                    ParticleSystem p = Instantiate(d.explosion, d.transform.position, Quaternion.identity);
                    Destroy(p.gameObject, 0.6f);
                    Destroy(d.gameObject);
                }
                );
                yield return new WaitForSeconds(t / 2);
            }
        }
    }

    IEnumerator Fly(float time)
    {
        _rb.AddForce(transform.forward + transform.up * 2f);
        JetScale();
        m_Axe.SetActive(true);
        m_isFlying = true;
        m_isJumping = true;
        animator.SetTrigger("Fly");
        yield return new WaitForSeconds(time);
        _rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        lines.transform.localScale = Vector3.zero;
        lines.transform.DOScale(Vector3.one, 0.3f);
        lines.gameObject.SetActive(true);
        lines.FetchPosition(startdimePos.position);
        currentSpeed = flySpeed;
    }

    IEnumerator Helmet(float time, bool isScaling)
    {
        float t = 0;
        m_currentHelmetMaterial = m_rend.materials[1];
        while (t < time)
        {
            t += Time.deltaTime;
            m_rend.materials[1].Lerp(m_currentHelmetMaterial, m_targetHelmetMaterial, t / time);
            m_rend.SetBlendShapeWeight(0, isScaling ? 100 - (t / time) * 100 : 100 - ((1 - (t / time)) * 100));
            yield return null;
        }
        m_targetHelmetMaterial = m_currentHelmetMaterial;
    }
    Coroutine helmetCorotine;
    void JetScale(bool isScale = true)
    {
        DOTween.Kill(this);
        if (helmetCorotine != null)
            StopCoroutine(helmetCorotine);
        helmetCorotine = StartCoroutine(Helmet(0.5f, isScale));
        if (isScale)
        {
            m_jetPack.SetActive(isScale);
            m_jetPack.transform.localScale = Vector3.zero;
            m_jetPack.transform.DOScale(m_startJetScale * 1.5f, 0.25f).SetEase(Ease.Flash).OnComplete(() =>
              {
                  m_jetPack.transform.DOScale(m_startJetScale, 0.2f).SetId(this);
                  m_wings.transform.DOScale(Vector3.one * 2, 0.2f).SetEase(Ease.InBounce).SetId(this).OnComplete(() => m_wings.transform.DOScale(Vector3.one, 0.2f)).SetId(this);
              }).SetId(this);
        }
        else
        {
            m_jetPack.transform.DOScale(m_startJetScale * 1.5f, 0.2f).SetEase(Ease.Flash).SetId(this).OnComplete(() =>
            {
                m_jetPack.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutBounce).SetId(this).OnComplete(() => m_jetPack.SetActive(isScale)).SetId(this);

            }).SetId(this);
        }
    }

    public void HideMaxUi()
    {
        m_maxText.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() => m_maxText.gameObject.SetActive(false)).SetId(this);
    }
    bool isSaking;
    public void ShakeMaxUi()
    {
        if (isSaking)
            return;
        Vector3 startRot = new Vector3(0, 180, 0);

        isSaking = true;
        Color startColor = m_maxText.color;
        m_maxText.DOColor(Color.red, 0.25f);
        m_maxText.rectTransform.DOScale(1.25f, 0.3f).SetEase(Ease.InElastic).OnComplete(() =>
        {
            m_maxText.rectTransform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutElastic).OnComplete(() => m_maxText.color = startColor);
            isSaking = false;
        });
    }
    public void TextPupUp()
    {
        DOTween.Kill(this);
        if (Root.GameManager.CurrentStorage >= Root.GameManager.MaxSacStorage)
        {
            m_maxText.gameObject.SetActive(true);
            m_maxText.transform.localScale = Vector3.zero;
            m_maxText.transform.DOScale(Vector3.one, 0.1f);
        }
        else
        {

            currentTxtject.gameObject.SetActive(true);
            currentTxtject.transform.localScale = Vector3.one * 0.8f;
            currentTxtject.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.Flash).OnComplete(() =>
            {
                currentTxtject.transform.DOScale(Vector3.one * 0.5f, 0.1f).OnComplete(() => currentTxtject.gameObject.SetActive(false));
            });
        }
    }

    public IEnumerator SmoothTurn(float time, Transform currentObj)
    {
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            Vector3 lookPos = currentObj.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, t / time);
            yield return null;

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position + Vector3.up * 0.05f, Vector3.down * m_checkDistance);
    }

    #region Animation Events
    internal void AddtheForce()
    {
        //rb.AddForce(Vector3.up * force, ForceMode.Impulse);
        m_isJumping = false;
    }
    //internal void PlayFlyFx()
    //{

    //    if (m_joystick.Direction.magnitude > 0.1f)
    //    {
    //        foreach (ParticleSystem particle in m_FlyFx)
    //        {
    //            particle.Play(true);
    //        }
    //    }
    //}
    internal void HitFx()
    {
        if (currentRock != null)
        {
            (int, int) Colected = currentRock.Hited(1, this);
            ShowColectiblesUi(Colected);
            //Root.GameManager.CurrentPlayerGold += Colected.Item2;
            ParticleSystem p = Instantiate(m_hitRockFx, m_hitRockFxPos);
            p.transform.localPosition = Vector3.zero;
            StartCoroutine(SmoothTurn(0.3f, currentRock.transform));
            p.transform.parent = null;
            Destroy(p.gameObject, 0.5f);
            Attack(Root.GameManager.CurrentStorage >= Root.GameManager.MaxSacStorage ? false : true);
        }
    }

    void ShowColectiblesUi((int, int) colectible)
    {
        if (currentRock.rockType == RockType.jem)
        {
            currentTxtject = m_UijemsCounter.gameObject;
            m_UijemsCounter.text = "+" + colectible.Item1.ToString();
            Root.GameManager.CurrentPlayerJems += colectible.Item1;
            Root.GameManager.CurrentStorage++;
        }
        else if (currentRock.rockType == RockType.oil)
        {
            currentTxtject = m_UiOilCounter.gameObject;
            m_UiOilCounter.text = "+" + colectible.Item1.ToString();
            Root.GameManager.CurrentStorage++;
            Root.GameManager.CurrentPlayerOil += colectible.Item1;
        }
        else if (currentRock.rockType == RockType.elecricity)
        {
            currentTxtject = m_UiElectricityCounter.gameObject;
            m_UiElectricityCounter.text = "+" + colectible.Item1.ToString();
            Root.GameManager.CurrentPlayerElectricity += colectible.Item1;
            Root.GameManager.CurrentStorage++;
        }
    }
    internal void OnMovmentAllowed()
    {
        m_isJumping = false;
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "FlyPoint" && !m_isFlying && isGrounded)
        //{
        //    StartCoroutine(Fly(0.2f));
        //    return;
        //}
        if (other.tag == "FlyPoint" && m_isFlying && !m_isJumping && !isGrounded)
        {
            m_Axe.SetActive(false);
            m_isFlying = false;
            m_isJumping = true;
            animator.SetTrigger("Landing");
            JetScale(false);
            lines.gameObject.SetActive(false);
            currentSpeed = speed;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        if (other.tag == "Reactor")
        {
            if (Root.GameManager.currentLevel == 0)
                Root.GameManager.EndTaksTutorial(9);

            if (Root.GameManager.CurrentPlayerOil <= 0)
                return;
            Reactor reactor = other.GetComponent<Reactor>();
            if (!Root.GameManager.isBaseCharged)
            {
                reactor.m_rocket.ShowRecharging();
                return;
            }

            if (reactor.isFull || reactor.isFilling)
                return;

            canputInReactor = true;
            int count = Root.GameManager.CurrentPlayerOil;//Mathf.Clamp(Root.GameManager.CurrentPlayerOil, 0, reactor.maxResouceCount - reactor.currentResourceLevel);
            StartCoroutine(FillReactor(0.5f, count, reactor));
        }

        if (other.tag == "UpgradeArea")
        {
            upgradeArea = other.GetComponent<UpgradeArea>();
            if (upgradeArea == null)
                return;
            upgradeArea.isInZone = true;
            StartCoroutine(upgradeArea.OpenUpGrade());
        }

        if (other.tag == "Seller")
        {
            m_sellpoint = other.GetComponent<SellPoint>();
            //if (!Root.GameManager.isBaseCharged)
            //{
            //    m_sellpoint.ShowRecharging();
            //}

            if (Root.GameManager.CurrentPlayerJems <= 0 || isColecting)
                return;

            m_sellpoint.isInZone = true;
            StartCoroutine(m_sellpoint.Sell(Root.GameManager.CurrentPlayerJems, this));
        }

        if (other.tag == "Mony")
        {
            stackMony = other.GetComponentInParent<StuckMony>();
            if (isColecting)
                return;

            isColecting = true;
            StartCoroutine(stackMony.ColectMony(this));
        }
        if (other.tag == "Generator")
        {
            if (Root.GameManager.CurrentPlayerElectricity <= 0)
                return;
            generator = other.GetComponentInParent<ElectricityGenerator>();
            generator.isInZone = true;
            StartCoroutine(generator.OpenGenerator(this));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "FlyPoint" && !m_isFlying && !isGrounded)
        {
            StartCoroutine(Fly(0.0f));
            return;
        }

        if (other.tag == "FlyPoint" && m_isFlying && isGrounded && !m_isJumping)
        {
            m_Axe.SetActive(false);
            m_isFlying = false;
            m_isJumping = true;
            animator.SetTrigger("Landing");
            JetScale(false);
            lines.gameObject.SetActive(false);
            currentSpeed = speed;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        if (other.tag == "Reactor")
        {
            canputInReactor = false;
        }
        if (other.tag == "UpgradeArea")
        {
            Root.Upgrademanager.Close();
            upgradeArea.isInZone = false;
            upgradeArea = null;
        }

        if (other.tag == "Seller")
        {
            if (m_sellpoint == null)
                return;

            m_sellpoint.isInZone = false;
            m_sellpoint = null;
        }
        if (other.tag == "Mony")
        {
            stackMony.isColecting = false;
        }
        if (other.tag == "Generator")
        {
            if (generator == null)
                return;

            generator.isInZone = false;
            generator = null;
        }

    }
    public void UpgradeJet(int maxUpgrade)
    {
        float speedUpgrade = 0.6f / maxUpgrade;
        flySpeed += speedUpgrade;
    }
    public void UpgradeAxe(float maxspeed, int damage = 0)
    {
        axeDamage += damage;
        axeSpeed += maxspeed <= 0 ? 0 : 1 / maxspeed;
        animator.SetFloat("AxeSpeed", axeSpeed);
    }
    public void UpgradeSac(int sacStorage)
    {
        Root.GameManager.MaxSacStorage += sacStorage;
    }
    public void SendJemsToTarget(Transform target)
    {
        Dimands d = Instantiate(dime, transform.position, Quaternion.identity);

        d.transform.DOJump(target.position, 0.8f, 1, 0.35f).OnComplete(() =>
        {
            Destroy(d.gameObject);
        });
    }
    public void ActivateUpgradeLvlFx(bool state)
    {
        upgradeFX.gameObject.SetActive(state);
    }
    public void Attack(bool isHiting) => animator.SetBool("IsHiting", isHiting);

    public void GoInRocket() => animator.SetTrigger("GoIn");

}
