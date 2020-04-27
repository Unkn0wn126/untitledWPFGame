using Engine.Models.Components.Script.BattleState;
using Engine.Models.Scenes;
using System.Windows.Controls;
using WPFGame.UI.BattleScreen.CommandConsole.AttackDirectionMenu;
using WPFGame.UI.BattleScreen.CommandConsole.AttackTypeMenu;
using WPFGame.UI.BattleScreen.CommandConsole.MovementTypeMenu;
using WPFGame.UI.MainMenu;

namespace WPFGame.UI.BattleScreen.CommandConsole
{
    /// <summary>
    /// Interaction logic for CommandConsoleOverlay.xaml
    /// </summary>
    public partial class CommandConsoleOverlay : UserControl
    {
        private AttackTypeSubMenu _attackTypeSubMenu;
        private AttackDirectionSubMenu _attackDirectionSubMenu;
        private MovementTypeSubMenu _movementTypeSubMenu;

        private BattleSceneMediator _battleSceneMediator;

        public CommandConsoleOverlay(BattleSceneMediator battleSceneMediator)
        {
            InitializeComponent();
            _battleSceneMediator = battleSceneMediator;
            _attackDirectionSubMenu = new AttackDirectionSubMenu(new ProcessMenuButtonClick(SetAttackDirectionToHead), 
                new ProcessMenuButtonClick(SetAttackDirectionToLeft), new ProcessMenuButtonClick(SetAttackDirectionToRight), 
                new ProcessMenuButtonClick(SetAttackDirectionToBottom), new ProcessMenuBackButtonClick(LoadPreviousMenu));

            _movementTypeSubMenu = new MovementTypeSubMenu(new ProcessMenuButtonClick(SetMovementStateToPass), 
                new ProcessMenuButtonClick(SetMovementStateToHeal), 
                new ProcessMenuButtonClick(SetMovementStateToAttack));

            _attackTypeSubMenu = new AttackTypeSubMenu(new ProcessMenuButtonClick(SetAttackTypeToNormal), 
                new ProcessMenuButtonClick(SetAttackTypeToHeavy), 
                new ProcessMenuBackButtonClick(LoadPreviousMenu));

            RestoreDefaultState();
        }

        private void LoadPreviousMenu(UserControl control)
        {
            if (control == _attackDirectionSubMenu)
            {
                _battleSceneMediator.PlayerBattleState.AttackDirection = AttackDirection.None;
                AddComponent(_attackTypeSubMenu);
            }
            else if (control == _attackTypeSubMenu)
            {
                _battleSceneMediator.PlayerBattleState.AttackType = AttackType.None;
                AddComponent(_movementTypeSubMenu);
            }

            RemoveComponent(control);
        }

        private void SetMovementStateToPass()
        {
            _battleSceneMediator.PlayerBattleState.MovementType = MovementType.None;
            _battleSceneMediator.PlayerBattleState.AttackType = AttackType.None;
            _battleSceneMediator.PlayerBattleState.AttackDirection = AttackDirection.None;
            _battleSceneMediator.PlayerBattleState.TurnDecided = true;
        }

        private void SetMovementStateToHeal()
        {
            _battleSceneMediator.PlayerBattleState.MovementType = MovementType.Heal;
            _battleSceneMediator.PlayerBattleState.AttackType = AttackType.None;
            _battleSceneMediator.PlayerBattleState.AttackDirection = AttackDirection.None;
            _battleSceneMediator.PlayerBattleState.TurnDecided = true;
        }

        private void SetMovementStateToAttack()
        {
            _battleSceneMediator.PlayerBattleState.MovementType = MovementType.Attack;

            RemoveComponent(_movementTypeSubMenu);
            AddComponent(_attackTypeSubMenu);
        }

        private void SetAttackTypeToNormal()
        {
            ProcessAttackTypeMenu(AttackType.Normal);
        }

        private void SetAttackTypeToHeavy()
        {
            ProcessAttackTypeMenu(AttackType.Heavy);
        }

        private void SetAttackDirectionToHead()
        {
            ProcessAttacktDirectionMenu(AttackDirection.Head);
        }

        private void SetAttackDirectionToBottom()
        {
            ProcessAttacktDirectionMenu(AttackDirection.Bottom);
        }

        private void SetAttackDirectionToLeft()
        {
            ProcessAttacktDirectionMenu(AttackDirection.Left);
        }

        private void SetAttackDirectionToRight()
        {
            ProcessAttacktDirectionMenu(AttackDirection.Right);
        }

        private void ProcessAttacktDirectionMenu(AttackDirection direction)
        {
            SetAttackDirectionToValue(direction);
            _battleSceneMediator.PlayerBattleState.TurnDecided = true;
            RemoveComponent(_attackDirectionSubMenu);
            AddComponent(_movementTypeSubMenu);
        }

        private void SetAttackDirectionToValue(AttackDirection direction)
        {
            _battleSceneMediator.PlayerBattleState.AttackDirection = direction;
        }

        private void ProcessAttackTypeMenu(AttackType type)
        {
            SetAttackTypeToValue(type);
            RemoveComponent(_attackTypeSubMenu);
            AddComponent(_attackDirectionSubMenu);
        }

        private void SetAttackTypeToValue(AttackType type)
        {
            _battleSceneMediator.PlayerBattleState.AttackType = type;
        }

        private void RemoveComponent(UserControl component)
        {
            if (MainGrid.Children.Contains(component))
            {
                MainGrid.Children.Remove(component);
            }
        }

        private void AddComponent(UserControl component)
        {
            if (!MainGrid.Children.Contains(component))
            {
                MainGrid.Children.Add(component);
            }

        }

        public void RestoreDefaultState()
        {
            RemoveComponent(_attackDirectionSubMenu);
            RemoveComponent(_movementTypeSubMenu);
            RemoveComponent(_attackTypeSubMenu);

            AddComponent(_movementTypeSubMenu);
        }
    }
}
