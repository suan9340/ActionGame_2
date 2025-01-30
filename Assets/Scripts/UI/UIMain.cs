using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIMain : MonoBehaviour
{
    [SerializeField] private Image damageImage = null;
    [SerializeField] private Slider playerHpBar = null;
    [SerializeField] private Slider playerMpBar = null;

    private Coroutine damageCor = null;
    private Coroutine hpaBarCor = null;
    private Coroutine mpBarCor = null;

    public void Initialize()
    {
        damageImage.gameObject.SetActive(false);
        playerHpBar.value = 1;
        playerMpBar.value = 1;
    }

    #region PlayerStat 관련
    public void OnDamageStart(float _curHp)     // 데미지 입었을 때
    {
        StartDamageCorutine();
        StartPlayerHPBarCorutine(_curHp);
    }

    private void StartDamageCorutine()
    {
        if (damageCor != null)
        {
            StopCoroutine(damageCor);
            damageCor = null;
        }
        damageCor = StartCoroutine(DamageCoroutine());
    }

    public void StartPlayerHPBarCorutine(float _curHp)
    {
        if (hpaBarCor != null)
        {
            StopCoroutine(hpaBarCor);
            hpaBarCor = null;
        }

        float _maxHp = GameConfigManager.Instance.GetPlayerData.maxHp;
        float _startValue = playerHpBar.value;

        hpaBarCor = StartCoroutine(MoveSlider(_maxHp, _startValue, _curHp, playerHpBar));
    }

    public void StartPlayerMpCoroutine(float _curMp)
    {
        if (mpBarCor != null)
        {
            StopCoroutine(mpBarCor);
            mpBarCor = null;
        }

        float _maxMp = GameConfigManager.Instance.GetPlayerData.maxMP;
        float _startValue = playerMpBar.value;

        mpBarCor = StartCoroutine(MoveSlider(_maxMp, _startValue, _curMp, playerMpBar));
    }

    private IEnumerator DamageCoroutine()
    {
        damageImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        damageImage.gameObject.SetActive(false);
        yield break;
    }

    private IEnumerator MoveSlider(float _maxHp, float _startValue, float _curValue, Slider _slider)
    {
        float _duration = 0.5f;
        float _time = 0;
        float _endValue = _curValue / _maxHp;

        while (_time < _duration)
        {
            _slider.value = Mathf.Lerp(_startValue, _endValue, _time / _duration);
            _time += Time.deltaTime;
            yield return null;
        }

        _slider.value = _endValue;
        yield break;
    }
    #endregion
}
