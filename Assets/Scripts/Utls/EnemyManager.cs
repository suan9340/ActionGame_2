using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviourSingleton<EnemyManager>
{
    [Header("利 府胶飘")]
    [SerializeField] private List<EnemyBase> enemyList = new List<EnemyBase>();

    [Space(10), Header("利 积己 罚待 矫埃")]
    [SerializeField] private Vector2 randTime = Vector2.one;

    [Space(10), Header("利 积己 罚待 困摹")]
    [SerializeField] private Vector2 randPosX = Vector2.one;
    [SerializeField] private Vector2 randPosZ = Vector2.one;

    private Coroutine enemyCor = null;

    private void Start()
    {
        
    }

    public void Init()
    {
        EnemyInstantiate();
    }
    private void EnemyInstantiate()
    {
        if (enemyCor != null)
        {
            StopCoroutine(enemyCor);
            enemyCor = null;
        }

        enemyCor = StartCoroutine(EnemyCoroutine());
    }

    private IEnumerator EnemyCoroutine()
    {

        while (true)
        {
            float _time = Random.Range(randTime.x, randTime.y);
            int _randEnemyIdx = Random.Range(0, enemyList.Count);

            Vector2 _randPos = new Vector2(Random.Range(randPosX.x, randPosX.y)
                , Random.Range(randPosZ.x, randPosZ.y));

            EnemyBase _enemy = enemyList[_randEnemyIdx];
            var _obj = Instantiate(_enemy, transform);
            _obj.Initialize();
            _obj.transform.localPosition = new Vector3(_randPos.x, 0f, _randPos.y);

            Debug.Log($"{_obj.name} 积己壩");
            yield return new WaitForSeconds(_time);
        }
    }
}
