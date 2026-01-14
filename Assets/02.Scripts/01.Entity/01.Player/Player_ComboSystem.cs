using System;


public class Player_ComboSystem : BaseBehaviour
{
    public event Action<int, int> OnComboChanged; // (현재 콤보, 증가량)

    private int _currentCombo;
    public int CurrentCombo { get { return _currentCombo; } }


    public void AddCombo(int amount = 1)
    {
        if (amount == 0)
            return;
        _currentCombo += amount;
        OnComboChanged?.Invoke(_currentCombo, amount);
    }

    public void ResetCombo()
    {
        if (_currentCombo == 0)
            return;

        _currentCombo = 0;
        OnComboChanged?.Invoke(_currentCombo, 0);
    }
}
