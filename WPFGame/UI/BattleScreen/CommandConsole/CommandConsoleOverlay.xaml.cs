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
            _attackDirectionSubMenu = new AttackDirectionSubMenu();
            _movementTypeSubMenu = new MovementTypeSubMenu(new ProcessMenuButtonClick(SetMovementStateToPass));
            _attackTypeSubMenu = new AttackTypeSubMenu();

            RestoreDefaultState();
        }

        private void SetMovementStateToPass()
        {
            _battleSceneMediator.PlayerBattleState.MovementType = MovementType.None;
            _battleSceneMediator.PlayerBattleState.TurnDecided = true;
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
