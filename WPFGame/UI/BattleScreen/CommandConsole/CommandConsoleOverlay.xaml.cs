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
        private readonly AttackTypeSubMenu _attackTypeSubMenu;
        private readonly AttackDirectionSubMenu _attackDirectionSubMenu;
        private readonly MovementTypeSubMenu _movementTypeSubMenu;

        private readonly BattleSceneMediator _battleSceneMediator;

        public CommandConsoleOverlay(BattleSceneMediator battleSceneMediator)
        {
            InitializeComponent();
            _battleSceneMediator = battleSceneMediator;
            _attackDirectionSubMenu = new AttackDirectionSubMenu(
                new ProcessMenuButtonClick(SetAttackDirectionToHead), new ProcessMenuButtonClick(SetAttackDirectionToLeft), 
                new ProcessMenuButtonClick(SetAttackDirectionToRight), new ProcessMenuButtonClick(SetAttackDirectionToBottom), 
                new ProcessMenuBackButtonClick(LoadPreviousMenu));

            _movementTypeSubMenu = new MovementTypeSubMenu(new ProcessMenuButtonClick(SetMovementStateToPass), 
                new ProcessMenuButtonClick(SetMovementStateToHeal), new ProcessMenuButtonClick(SetMovementStateToAttack));

            _attackTypeSubMenu = new AttackTypeSubMenu(new ProcessMenuButtonClick(SetAttackTypeToNormal), 
                new ProcessMenuButtonClick(SetAttackTypeToHeavy), new ProcessMenuBackButtonClick(LoadPreviousMenu));

            RestoreDefaultState();
        }

        /// <summary>
        /// Updates the available
        /// battle actions based
        /// on the current context
        /// </summary>
        public void UpdateButtonAvailability()
        {
            _movementTypeSubMenu.AttackButton.IsEnabled = _battleSceneMediator.PlayerBattleState.IsOnTurn &&
                _battleSceneMediator.PlayerBattleState.CanAttack();

            _movementTypeSubMenu.HealButton.IsEnabled = _battleSceneMediator.PlayerBattleState.IsOnTurn && 
                _battleSceneMediator.PlayerBattleState.CanHeal();

            _movementTypeSubMenu.PassButton.IsEnabled = _battleSceneMediator.PlayerBattleState.IsOnTurn;
        }

        /// <summary>
        /// Loads the previous menu
        /// </summary>
        /// <param name="control"></param>
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

        /// <summary>
        /// Sets the battle movement to pass
        /// </summary>
        private void SetMovementStateToPass()
        {
            _battleSceneMediator.PlayerBattleState.MovementType = MovementType.None;
            _battleSceneMediator.PlayerBattleState.AttackType = AttackType.None;
            _battleSceneMediator.PlayerBattleState.AttackDirection = AttackDirection.None;
            _battleSceneMediator.PlayerBattleState.TurnDecided = true;
        }

        /// <summary>
        /// Sets the battle movement to heal
        /// </summary>
        private void SetMovementStateToHeal()
        {
            _battleSceneMediator.PlayerBattleState.MovementType = MovementType.Heal;
            _battleSceneMediator.PlayerBattleState.AttackType = AttackType.None;
            _battleSceneMediator.PlayerBattleState.AttackDirection = AttackDirection.None;
            _battleSceneMediator.PlayerBattleState.TurnDecided = true;
        }

        /// <summary>
        /// Sets the battle movement to attack
        /// </summary>
        private void SetMovementStateToAttack()
        {
            _battleSceneMediator.PlayerBattleState.MovementType = MovementType.Attack;

            RemoveComponent(_movementTypeSubMenu);
            AddComponent(_attackTypeSubMenu);
        }

        /// <summary>
        /// Sets the attack type to normal
        /// </summary>
        private void SetAttackTypeToNormal()
        {
            ProcessAttackTypeMenu(AttackType.Normal);
        }

        /// <summary>
        /// Sets the attack type to heavy
        /// </summary>
        private void SetAttackTypeToHeavy()
        {
            ProcessAttackTypeMenu(AttackType.Heavy);
        }

        /// <summary>
        /// Sets the attack direction to head
        /// </summary>
        private void SetAttackDirectionToHead()
        {
            ProcessAttacktDirectionMenu(AttackDirection.Head);
        }

        /// <summary>
        /// Sets the attack direction to bottom
        /// </summary>
        private void SetAttackDirectionToBottom()
        {
            ProcessAttacktDirectionMenu(AttackDirection.Bottom);
        }

        /// <summary>
        /// Sets the attack direction to left
        /// </summary>
        private void SetAttackDirectionToLeft()
        {
            ProcessAttacktDirectionMenu(AttackDirection.Left);
        }

        /// <summary>
        /// Sets the attack direction to right
        /// </summary>
        private void SetAttackDirectionToRight()
        {
            ProcessAttacktDirectionMenu(AttackDirection.Right);
        }

        /// <summary>
        /// Sets the attack direction to a given value,
        /// informs the logic that the turn has been
        /// decided and loads back the initial
        /// movement type options menu
        /// </summary>
        /// <param name="direction"></param>
        private void ProcessAttacktDirectionMenu(AttackDirection direction)
        {
            SetAttackDirectionToValue(direction);
            _battleSceneMediator.PlayerBattleState.TurnDecided = true;
            RemoveComponent(_attackDirectionSubMenu);
            AddComponent(_movementTypeSubMenu);
        }

        /// <summary>
        /// Sets the attack direction to a given value
        /// </summary>
        /// <param name="direction"></param>
        private void SetAttackDirectionToValue(AttackDirection direction)
        {
            _battleSceneMediator.PlayerBattleState.AttackDirection = direction;
        }

        /// <summary>
        /// Sets the attack type to a given value
        /// and loads the attack direction options menu
        /// </summary>
        /// <param name="type"></param>
        private void ProcessAttackTypeMenu(AttackType type)
        {
            SetAttackTypeToValue(type);
            RemoveComponent(_attackTypeSubMenu);
            AddComponent(_attackDirectionSubMenu);
        }

        /// <summary>
        /// Sets the attack type to a given value
        /// </summary>
        /// <param name="type"></param>
        private void SetAttackTypeToValue(AttackType type)
        {
            _battleSceneMediator.PlayerBattleState.AttackType = type;
        }

        /// <summary>
        /// Helper method to remove a component
        /// only if already present
        /// </summary>
        /// <param name="component"></param>
        private void RemoveComponent(UserControl component)
        {
            if (MainGrid.Children.Contains(component))
            {
                MainGrid.Children.Remove(component);
            }
        }

        /// <summary>
        /// Helper method to add a component
        /// only if not already present
        /// </summary>
        /// <param name="component"></param>
        private void AddComponent(UserControl component)
        {
            if (!MainGrid.Children.Contains(component))
            {
                MainGrid.Children.Add(component);
            }

        }

        /// <summary>
        /// Restores the default state of this menu
        /// </summary>
        public void RestoreDefaultState()
        {
            RemoveComponent(_attackDirectionSubMenu);
            RemoveComponent(_movementTypeSubMenu);
            RemoveComponent(_attackTypeSubMenu);

            AddComponent(_movementTypeSubMenu);
        }
    }
}
